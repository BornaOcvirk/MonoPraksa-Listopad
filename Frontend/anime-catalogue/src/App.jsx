import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Login from './components/Login'
import Register from './components/Register'
import Layout from './components/Layout'
import Home from './components/Home'
import AddAnime from './components/AddAnime'
import EditAnime from './components/EditAnime'
import AnimeDetails from './components/AnimeDetails'
import StudioList from './components/StudioList'
import StudioDetails from './components/StudioDetails'
import MangaList from './components/MangaList'
import MangaDetails from './components/MangaDetails'
import UserList from './components/UserList'
import Profile from './components/Profile'
import AdminRoute from './components/AdminRoute'
import './App.css'

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Pages below share the header and require a logged-in user */}
                <Route element={<Layout />}>
                    <Route path="/" element={<Home />} />
                    <Route path="/anime/new" element={<AdminRoute><AddAnime /></AdminRoute>} />
                    <Route path="/anime/:id" element={<AnimeDetails />} />
                    <Route path="/anime/:id/edit" element={<AdminRoute><EditAnime /></AdminRoute>} />
                    <Route path="/studios" element={<StudioList />} />
                    <Route path="/studios/:id" element={<StudioDetails />} />
                    <Route path="/mangas" element={<MangaList />} />
                    <Route path="/mangas/:id" element={<MangaDetails />} />
                    <Route path="/users" element={<AdminRoute><UserList /></AdminRoute>} />
                    <Route path="/profile" element={<Profile />} />
                </Route>

                <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App
