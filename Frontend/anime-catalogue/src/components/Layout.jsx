import { Link, Navigate, NavLink, Outlet, useNavigate } from 'react-router-dom'
import { getCurrentUser, logout } from './api'

// Shared header + login check for every page except Login/Register
function Layout() {
    const user = getCurrentUser()
    const navigate = useNavigate()

    if (!user) {
        return <Navigate to="/login" replace />
    }

    function handleLogout() {
        logout()
        navigate('/login')
    }

    return (
        <div className="page">
            <header className="header">
                <h1>Anime Catalogue</h1>
                <nav className="nav">
                    <NavLink to="/" end>Catalogue</NavLink>
                    <NavLink to="/studios">Studios</NavLink>
                    <NavLink to="/mangas">Mangas</NavLink>
                    {user.role === 'Admin' && <NavLink to="/users">Users</NavLink>}
                </nav>
                <div className="header-actions">
                    <Link to="/profile">{user.username} ({user.role})</Link>
                    <button onClick={handleLogout}>Log out</button>
                </div>
            </header>

            <Outlet />
        </div>
    )
}

export default Layout
