async function request(path, { method = 'GET', body } = {}) {
    const token = localStorage.getItem('token')

    const res = await fetch(path, {
        method,
        headers: {
            ...(body && { 'Content-Type': 'application/json' }),
            ...(token && { Authorization: `Bearer ${token}` }),
        },
        body: body ? JSON.stringify(body) : undefined,
    })

    // The backend sends errors as plain text, e.g. "Wrong username or password."
    const text = await res.text()
    let data
    try { data = text ? JSON.parse(text) : null } catch { data = text }

    if (!res.ok) {
        throw new Error(typeof data === 'string' && data ? data : data?.title || `Error ${res.status}`)
    }
    return data
}

// Backend returns { token, expiresAt, user: { user_id, username, role } }
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