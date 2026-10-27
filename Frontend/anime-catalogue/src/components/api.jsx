/* eslint-disable react-refresh/only-export-components */

const API_URL = import.meta.env.VITE_API_URL

async function request(path, { method = 'GET', body, query } = {}) {
    let qs = ''
    if (query) {
        const clean = Object.fromEntries(
            Object.entries(query).filter(([, v]) => v !== undefined && v !== null && v !== '')
        )
        const params = new URLSearchParams(clean).toString()
        if (params) qs = '?' + params
    }

    const token = localStorage.getItem('token')

    const res = await fetch(API_URL + path + qs, {
        method,
        headers: {
            ...(body && { 'Content-Type': 'application/json' }),
            ...(token && { Authorization: `Bearer ${token}` }),
        },
        body: body ? JSON.stringify(body) : undefined,
    })

    const text = await res.text()
    let data
    try { data = text ? JSON.parse(text) : null } catch { data = text }

    if (!res.ok) {
        let message = typeof data === 'string' && data ? data : data?.title || `Error ${res.status}`
        if (res.status === 401 && !text) message = 'You need to log in.'
        if (res.status === 403) message = 'Only admins can do this.'
        const error = new Error(message)
        error.status = res.status
        throw error
    }
    return data
}

function saveSession(result) {
    localStorage.setItem('token', result.token)
    localStorage.setItem('user', JSON.stringify(result.user))
    return result.user
}

export async function login(username, password) {
    const result = await request('/api/Auth/login', { method: 'POST', body: { username, password } })
    return saveSession(result)
}

export async function register(username, password) {
    const result = await request('/api/Auth/register', { method: 'POST', body: { username, password } })
    return saveSession(result)
}

export function logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
}

export function getCurrentUser() {
    try { return JSON.parse(localStorage.getItem('user')) } catch { return null }
}

const ANIME = '/api/AnimeWebCatalogue'

export const searchAnime    = (query) => request(ANIME, { query })
export const getAnimeGenres = ()      => request(`${ANIME}/genres`)
export const getAnime       = (id)    => request(`${ANIME}/${id}`)

export const createAnime = (anime)     => request(ANIME, { method: 'POST', body: anime })
export const updateAnime = (id, anime) => request(`${ANIME}/${id}`, { method: 'PUT', body: anime })
export const deleteAnime = (id)        => request(`${ANIME}/${id}`, { method: 'DELETE' })

export const getStudios        = ()   => request('/api/Studio')
export const getStudio         = (id) => request(`/api/Studio/${id}`)
export const getMangas         = ()   => request('/api/Manga')
export const getManga          = (id) => request(`/api/Manga/${id}`)
export const getMangasByStudio = (id) => request(`/api/Manga/studio/${id}`)

export const getUsers   = ()   => request('/api/User')
export const deleteUser = (id) => request(`/api/User/${id}`, { method: 'DELETE' })
export const getMe      = ()   => request('/api/Auth/me')
