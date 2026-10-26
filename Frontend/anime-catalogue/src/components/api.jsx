/* eslint-disable react-refresh/only-export-components */

// Backend address, read from the .env file (VITE_API_URL=http://localhost:5113)
const API_URL = import.meta.env.VITE_API_URL

async function request(path, { method = 'GET', body, query } = {}) {
    // { search: 'titan', genre: '' } → ?search=titan (empty values are skipped)
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

    // The backend sends errors as plain texssword."
    const text = await res.text()
    let data
    try { data = text ? JSON.parse(text) : null } catch { data = text }

    if (!res.ok) {
         throw new Error(typeof data === 'string' && data ? data : data?.title || `Error ${res.status}`)
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
export const searchAnime    = (query) => request('/api/AnimeWebCatalogue', { query })
export const getAnimeGenres = ()      => request('/api/AnimeWebCatalogue/genres')
export const getStudios = () => request('/api/Studio')