import { useState } from 'react'
import { getCurrentUser, getUsers, deleteUser, makeAdmin } from './api'
import useLoad from './useLoad'

// Admin only (wrapped in AdminRoute in App.jsx)
function UserList() {
    const me = getCurrentUser()
    const { data: users, error: loadError, setData: setUsers } = useLoad(getUsers)
    const [error, setError] = useState('')
    const [busyId, setBusyId] = useState(null) // row with a request in flight, to block double clicks

    async function handleDelete(user) {
        if (!window.confirm(`Delete user "${user.username}"? This can't be undone.`)) return
        setBusyId(user.user_id)
        try {
            await deleteUser(user.user_id)
            setUsers(list => list.filter(u => u.user_id !== user.user_id))
            setError('')
        } catch (err) {
            setError(err.message)
        } finally {
            setBusyId(null)
        }
    }

    async function handleMakeAdmin(user) {
        if (!window.confirm(`Make "${user.username}" an admin? They will be able to edit anime and manage users.`)) return
        setBusyId(user.user_id)
        try {
            const updated = await makeAdmin(user.user_id)
            setUsers(list => list.map(u => u.user_id === user.user_id ? updated : u))
            setError('')
        } catch (err) {
            setError(err.message)
        } finally {
            setBusyId(null)
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
                                            <div className="row-actions">
                                                {u.role !== 'Admin' && (
                                                    <button onClick={() => handleMakeAdmin(u)}
                                                        disabled={busyId === u.user_id}
                                                        aria-label={`Make ${u.username} an admin`}>
                                                        Make admin
                                                    </button>
                                                )}
                                                <button className="danger" onClick={() => handleDelete(u)}
                                                    disabled={busyId === u.user_id}
                                                    aria-label={`Delete user ${u.username}`}>
                                                    Delete
                                                </button>
                                            </div>
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
