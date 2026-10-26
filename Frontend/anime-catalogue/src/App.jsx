import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Login from './components/Login'
import Register from './components/Register'
import Home from './components/Home'
import AddAnime from './components/AddAnime'
import EditAnime from './components/EditAnime'
import AdminRoute from './components/AdminRoute'
import './App.css'

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/anime/new" element={<AdminRoute><AddAnime /></AdminRoute>} />
                <Route path="/anime/:id/edit" element={<AdminRoute><EditAnime /></AdminRoute>} />
                <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App
