import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { getCurrentUser, searchAnime, getAnimeGenres, getStudios, deleteAnime } from './api'

const PAGE_SIZE = 6

function Home() {
    const user = getCurrentUser()
    const isAdmin = user?.role === 'Admin'

    const [search, setSearch] = useState('')
    const [author, setAuthor] = useState('')
    const [genre, setGenre] = useState('')
    const [studioId, setStudioId] = useState('')
    const [page, setPage] = useState(1)
    const [reloadKey, setReloadKey] = useState(0)

    const [animes, setAnimes] = useState([])
    const [totalCount, setTotalCount] = useState(0)
    const [totalPages, setTotalPages] = useState(1)
    const [genres, setGenres] = useState([])
    const [studios, setStudios] = useState([])
    const [error, setError] = useState('')

    useEffect(() => {
        getAnimeGenres().then(setGenres).catch(err => setError(err.message))
        getStudios().then(setStudios).catch(err => setError(err.message))
    }, [])

    useEffect(() => {
        let ignore = false

        searchAnime({ search, author, genre, studioId, page, pageSize: PAGE_SIZE })
            .then(result => {
                if (ignore) return
                setAnimes(result.items)
                setTotalCount(result.totalCount)
                setTotalPages(Math.max(result.totalPages, 1))
                setError('')
            })
            .catch(err => { if (!ignore) setError(err.message) })

        return () => { ignore = true }
    }, [search, author, genre, studioId, page, reloadKey])

    function clearFilters() {
        setSearch('')
        setAuthor('')
        setGenre('')
        setStudioId('')
        setPage(1)
    }

    async function handleDelete(anime) {
        if (!window.confirm(`Delete "${anime.name}"? This can't be undone.`)) return

        try {
            await deleteAnime(anime.id)
            if (animes.length === 1 && page > 1) {
                setPage(p => p - 1)
            } else {
                setReloadKey(k => k + 1)
            }
        } catch (err) {
            setError(err.message)
        }
    }

    return (
        <main>
            {isAdmin && (
                <Link to="/anime/new" className="button-link primary">+ Add anime</Link>
            )}

            <input
                className="search-bar"
                type="search"
                placeholder="Search anime by title..."
                value={search}
                onChange={e => { setSearch(e.target.value); setPage(1) }}
            />

            <div className="filters">
                <input
                    placeholder="Author"
                    value={author}
                    onChange={e => { setAuthor(e.target.value); setPage(1) }}
                />

                <select value={genre} onChange={e => { setGenre(e.target.value); setPage(1) }}>
                    <option value="">All genres</option>
                    {genres.map(g => (
                        <option key={g} value={g}>{g}</option>
                    ))}
                </select>

                <select value={studioId} onChange={e => { setStudioId(e.target.value); setPage(1) }}>
                    <option value="">All studios</option>
                    {studios.map(s => (
                        <option key={s.studioId} value={s.studioId}>{s.nameS}</option>
                    ))}
                </select>

                <button onClick={clearFilters}>Clear filters</button>
            </div>

            {error && <p className="error">{error}</p>}
            <p>{totalCount} anime found</p>

            <div className="grid">
                {animes.map(a => (
                    <div className="card anime-card" key={a.id}>
                        <h3><Link to={`/anime/${a.id}`}>{a.name}</Link></h3>
                        <p>{a.genre} · {a.numberOfSeasons} season(s)</p>
                        <p>Studio: <Link to={`/studios/${a.studioId}`}>{a.studioName}</Link></p>
                        <p>{a.mangaId
                            ? <>Manga: <Link to={`/mangas/${a.mangaId}`}>{a.mangaTitle}</Link> by {a.author}</>
                            : 'Original anime (no manga)'}</p>
                        <p>Released: {a.releaseDate}</p>

                        {isAdmin && (
                            <div className="card-actions">
                                <Link to={`/anime/${a.id}/edit`} className="button-link">Edit</Link>
                                <button className="danger" onClick={() => handleDelete(a)}>Delete</button>
                            </div>
                        )}
                    </div>
                ))}
            </div>

            <div className="pagination">
                <button disabled={page <= 1} onClick={() => setPage(p => p - 1)}>
                    ← Previous
                </button>
                <span>Page {page} of {totalPages}</span>
                <button disabled={page >= totalPages} onClick={() => setPage(p => p + 1)}>
                    Next →
                </button>
            </div>
        </main>
    )
}

export default Home
