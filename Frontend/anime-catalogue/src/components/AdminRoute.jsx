import { Navigate } from 'react-router-dom'
import { getCurrentUser } from './api'

function AdminRoute({ children }) {
    const user = getCurrentUser()

    if (!user) {
        return <Navigate to="/login" replace />
    }
    if (user.role !== 'Admin') {
        return <Navigate to="/" replace />
    }
    return children
}

export default AdminRoute
