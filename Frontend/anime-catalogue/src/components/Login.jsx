import { useState } from 'react'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import { getCurrentUser, login } from './api'

function Login() {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)
    const navigate = useNavigate()

    // Already logged in: skip the form and go to the catalogue
    if (getCurrentUser()) {
        return <Navigate to="/" replace />
    }

    async function handleSubmit(e) {
        e.preventDefault() // stop the page from reloading
        setError('')
        setLoading(true)
        try {
            await login(username, password)
            navigate('/') // go to Home after logging in
        } catch (err) {
            setError(err.message)
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-box">
            <h2>Log in</h2>
            <form onSubmit={handleSubmit}>
                <input
                    placeholder="Username"
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    required
                />
                {error && <p className="error">{error}</p>}
                <button type="submit" disabled={loading}>
                    {loading ? 'Logging in...' : 'Log in'}
                </button>
            </form>
            <p>No account? <Link to="/register">Register</Link></p>
        </div>
    )
}
export default Login