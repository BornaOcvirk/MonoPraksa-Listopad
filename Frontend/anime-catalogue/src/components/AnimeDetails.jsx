import { Fragment, useEffect, useState } from 'react'
import { Link, Navigate, useNavigate, useParams } from 'react-router-dom'
import { getCurrentUser, getAnime, getStudios, getMangas, deleteAnime } from './api'

// Loads one anime. If the endpoint only returns ids, look up the studio and manga names.
async function loadAnime(id) {
    const a = await getAnime(id)
    if (!a.studioName && a.studioId) {
        a.studioName = (await getStudios()).find(s => s.studioId === a.studioId)?.nameS
    }
    if (!a.mangaTitle && a.mangaId) {
        const manga = (await getMangas()).find(m => m.mangaId === a.mangaId)
        a.mangaTitle = manga?.title
        a.author = manga?.author
    }
    return a
}

function AnimeDetails() {
    const { id } = useParams()
    const navigate = useNavigate()
    const user = getCurrentUser()
    const [anime, setAnime] = useState(null)
    const [error, setError] = useState('')

    useEffect(() => {
        let ignore = false
        loadAnime(id)
            .then(a => { if (!ignore) setAnime(a) })
            .catch(err => { if (!ignore) setError(err.message) })
        return () => { ignore = true }
    }, [id])

    if (!user) {
        return <Navigate to="/login" replace />
    }

    async function handleDelete() {
        if (!window.confirm(`Delete "${anime.name}"? This can't be undone.`)) return
        try {
            await deleteAnime(id)
            navigate('/')
        } catch (err) {
            setError(err.message)
        }
    }

    const fields = anime && [
        ['Genre', anime.genre],
        ['Seasons', anime.numberOfSeasons],
        ['Price', Number(anime.price).toFixed(2)],
        ['Released', anime.releaseDate],
        ['Studio', anime.studioName],
        ['Popularity', anime.popularity || 'Not set'],
        ['Manga', anime.mangaTitle ? `${anime.mangaTitle} by ${anime.author}` : 'Original anime (no manga)'],
    ]

    return (
        <main className="page">
            <Link to="/">← Back to catalogue</Link>

            {error && <p className="error">{error}</p>}
            {!anime && !error && <p>Loading...</p>}

            {anime && (
                <article className="details">
                    <h1>{anime.name}</h1>
                    <dl>
                        {fields.map(([label, value]) => (
                            <Fragment key={label}>
                                <dt>{label}</dt>
                                <dd>{value}</dd>
                            </Fragment>
                        ))}
                    </dl>

                    {user.role === 'Admin' && (
                        <div className="card-actions">
                            <Link to={`/anime/${id}/edit`} className="button-link">Edit</Link>
                            <button className="danger" onClick={handleDelete}>Delete</button>
                        </div>
                    )}
                </article>
            )}
        </main>
    )
}

export default AnimeDetails
