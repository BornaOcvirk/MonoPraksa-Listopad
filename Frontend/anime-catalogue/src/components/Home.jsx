import { Navigate, useNavigate } from 'react-router-dom'
import { getCurrentUser, logout } from './api'

function Home() {
    const user = getCurrentUser()
    const navigate = useNavigate()

    // Not logged in → go to the login page
    if (!user) {
        return <Navigate to="/login" replace />
    }

    function handleLogout() {
        logout()
        navigate('/login')
    }

    return (
        <div className="auth-box">
            <h2>Welcome, {user.username}!</h2>
            <p>Role: {user.role}</p>
            <button onClick={handleLogout}>Log out</button>
        </div>
    )
}

export default Home