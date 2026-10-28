import { useCallback, useEffect, useLayoutEffect, useRef } from 'react'
import { Link, Navigate, NavLink, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { getCurrentUser, logout } from './api'

// Slides the gold bar under `link`, or fades it out when there is no link.
// data-dir tells the CSS which edge leads, so the bar stretches toward the target, then shrinks.
function moveBar(bar, link) {
    if (!bar) return
    if (!link) {
        bar.classList.remove('visible')
        return
    }

    const nav = link.parentElement
    const left = link.offsetLeft
    const right = nav.clientWidth - left - link.offsetWidth
    const top = link.offsetTop + link.offsetHeight - 3

    if (!bar.classList.contains('visible')) {
        // Appearing: jump straight to the link instead of sliding in from an old spot
        bar.style.transition = 'none'
        Object.assign(bar.style, { left: `${left}px`, right: `${right}px`, top: `${top}px` })
        void bar.offsetWidth // apply the jump before turning transitions back on
        bar.style.transition = ''
        bar.classList.add('visible')
        return
    }

    bar.dataset.dir = left < parseFloat(bar.style.left) ? 'left' : 'right'
    Object.assign(bar.style, { left: `${left}px`, right: `${right}px`, top: `${top}px` })
}

// Shared header + login check for every page except Login/Register
function Layout() {
    const user = getCurrentUser()
    const navigate = useNavigate()
    const { pathname } = useLocation()
    const navRef = useRef(null)
    const barRef = useRef(null)

    const showActive = useCallback(() => {
        moveBar(barRef.current, navRef.current?.querySelector('a.active'))
    }, [])

    // Back under the current page's link after navigating
    useLayoutEffect(() => { showActive() }, [pathname, showActive])

    // Link widths change when the window resizes or the web fonts finish loading
    useEffect(() => {
        window.addEventListener('resize', showActive)
        document.fonts?.ready.then(showActive)
        return () => window.removeEventListener('resize', showActive)
    }, [showActive])

    if (!user) {
        return <Navigate to="/login" replace />
    }

    function handleLogout() {
        logout()
        navigate('/login')
    }

    function handlePoint(e) {
        const link = e.target.closest('a')
        if (link) moveBar(barRef.current, link)
    }

    function handleBlur(e) {
        if (!navRef.current?.contains(e.relatedTarget)) showActive()
    }

    return (
        <div className="page">
            <header className="header">
                <h1>Anime Catalogue</h1>
                <nav className="nav" ref={navRef}
                    onMouseOver={handlePoint} onFocus={handlePoint}
                    onMouseLeave={showActive} onBlur={handleBlur}>
                    <NavLink to="/" end>Catalogue</NavLink>
                    <NavLink to="/studios">Studios</NavLink>
                    <NavLink to="/mangas">Mangas</NavLink>
                    {user.role === 'Admin' && <NavLink to="/users">Users</NavLink>}
                    <span className="nav-bar" ref={barRef} aria-hidden="true" />
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
