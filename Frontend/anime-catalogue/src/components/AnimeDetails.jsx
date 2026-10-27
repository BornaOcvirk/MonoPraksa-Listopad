import { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { getCurrentUser, getAnime, getStudios, getMangas, deleteAnime } from './api'
import Fields from './Fields'

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
        ['Released', anime.releaseDate],
        ['Studio', <Link to={`/studios/${anime.studioId}`}>{anime.studioName ?? 'View studio'}</Link>],
        ['Popularity', anime.popularity],
        ['Manga', anime.mangaId
            ? <Link to={`/mangas/${anime.mangaId}`}>{anime.mangaTitle} by {anime.author}</Link>
            : 'Original anime (no manga)'],
    ]

    return (
        <main>
            <Link to="/">← Back to catalogue</Link>

            {error && <p className="error">{error}</p>}
            {!anime && !error && <p>Loading...</p>}

            {anime && (
                <article className="details">
                    <h1>{anime.name}</h1>
                    <Fields fields={fields} />

                    {user?.role === 'Admin' && (
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
