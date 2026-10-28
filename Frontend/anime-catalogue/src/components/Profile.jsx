import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getMe, logout } from './api'
import Fields from './Fields'
import { SkeletonDetails } from './Skeleton'

function Profile() {
    const navigate = useNavigate()
    const [me, setMe] = useState(null)
    const [error, setError] = useState('')

    useEffect(() => {
        let ignore = false
        getMe()
            .then(result => { if (!ignore) setMe(result) })
            .catch(err => {
                if (ignore) return
                // Token expired or invalid: clear the session and send the user to log in again
                if (err.status === 401) {
                    logout()
                    navigate('/login', { replace: true })
                } else {
                    setError(err.message)
                }
            })
        return () => { ignore = true }
    }, [navigate])

    function handleLogout() {
        logout()
        navigate('/login')
    }

    return (
        <main>
            {error && <p className="error">{error}</p>}
            {!me && !error && <SkeletonDetails rows={3} />}

            {me && (
                <article className="details">
                    <h1>My account</h1>
                    <Fields fields={[
                        ['Username', me.username],
                        ['Role', me.role],
                        ['User ID', me.userId],
                    ]} />
                    <button onClick={handleLogout}>Log out</button>
                </article>
            )}
        </main>
    )
}

export default Profile
