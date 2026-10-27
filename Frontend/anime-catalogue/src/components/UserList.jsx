import { useState } from 'react'
import { getCurrentUser, getUsers, deleteUser } from './api'
import useLoad from './useLoad'

// Admin only (wrapped in AdminRoute in App.jsx)
function UserList() {
    const me = getCurrentUser()
    const { data: users, error: loadError, setData: setUsers } = useLoad(getUsers)
    const [error, setError] = useState('')

    async function handleDelete(user) {
        if (!window.confirm(`Delete user "${user.username}"? This can't be undone.`)) return
        try {
            await deleteUser(user.user_id)
            setUsers(list => list.filter(u => u.user_id !== user.user_id))
            setError('')
        } catch (err) {
            setError(err.message)
        }
    }

    return (
        <main>
            <h2>Users</h2>
            {(error || loadError) && <p className="error">{error || loadError}</p>}
            {!users && !loadError && <p>Loading...</p>}

            {users && (
                <div className="table-wrap">
                    <table className="table">
                        <thead>
                            <tr><th scope="col">Username</th><th scope="col">Role</th><th scope="col">Actions</th></tr>
                        </thead>
                        <tbody>
                            {users.map(u => (
                                <tr key={u.user_id}>
                                    <td>{u.username}</td>
                                    <td>{u.role}</td>
                                    <td>
                                        {u.user_id === me?.user_id ? '(you)' : (
                                            <button className="danger" onClick={() => handleDelete(u)}
                                                aria-label={`Delete user ${u.username}`}>
                                                Delete
                                            </button>
                                        )}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </main>
    )
}

export default UserList
