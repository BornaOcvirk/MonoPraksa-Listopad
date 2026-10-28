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
            // A 401 is already handled in request(): it logs out and redirects to /login
            .catch(err => { if (!ignore) setError(err.message) })
        return () => { ignore = true }
    }, [])

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
