import { useState } from 'react'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import { getCurrentUser, register } from './api'

const MIN_PASSWORD_LENGTH = 8

function Register() {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirmPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)
    const navigate = useNavigate()

    // Already logged in: skip the form and go to the catalogue
    if (getCurrentUser()) {
        return <Navigate to="/" replace />
    }

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')

        if (password.length < MIN_PASSWORD_LENGTH) {
            setError(`Password must be at least ${MIN_PASSWORD_LENGTH} characters.`)
            return
        }

        if (password !== confirmPassword) {
            setError('Passwords do not match.')
            return
        }

        setLoading(true)
        try {
            await register(username, password) // also logs you in
            navigate('/')
        } catch (err) {
            setError(err.message) // e.g. "Username is already taken."
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-box">
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <input
                    placeholder="Username (3–30 letters, numbers, _ . -)"
                    minLength={3}
                    maxLength={30}
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder={`Password (min. ${MIN_PASSWORD_LENGTH} characters)`}
                    minLength={MIN_PASSWORD_LENGTH}
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Confirm password"
                    value={confirmPassword}
                    onChange={e => setConfirmPassword(e.target.value)}
                    required
                />
                {error && <p className="error">{error}</p>}
                <button type="submit" disabled={loading}>
                    {loading ? 'Creating account...' : 'Register'}
                </button>
            </form>
            <p>Already have an account? <Link to="/login">Log in</Link></p>
        </div>
    )
}

export default Register