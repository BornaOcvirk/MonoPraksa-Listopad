import { useEffect, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { getCurrentUser, searchAnime, getAnimeGenres, getStudios, deleteAnime } from './api'
import { SkeletonCards } from './Skeleton'

const PAGE_SIZE = 6

function Home() {
    const user = getCurrentUser()
    const isAdmin = user?.role === 'Admin'

    // Page and applied filters live in the URL, so the browser Back button returns to the same page
    const [params, setParams] = useSearchParams()
    const search = params.get('search') ?? ''
    const author = params.get('author') ?? ''
    const genre = params.get('genre') ?? ''
    const studioId = params.get('studio') ?? ''
    const page = Math.max(Math.floor(Number(params.get('page'))) || 1, 1) // ignores junk like ?page=abc or -3

    // What the user is typing; only sent to the API when they press Search or Enter
    const [searchInput, setSearchInput] = useState(search)
    const [authorInput, setAuthorInput] = useState(author)
    const [reloadKey, setReloadKey] = useState(0)

    // When Back/Forward changes the URL's search, show that text in the boxes again
    const [shownFilters, setShownFilters] = useState({ search, author })
    if (shownFilters.search !== search || shownFilters.author !== author) {
        setShownFilters({ search, author })
        setSearchInput(search)
        setAuthorInput(author)
    }

    // Empty values and page 1 are left out to keep the URL short
    function updateParams(changes) {
        const next = new URLSearchParams(params)
        for (const [key, value] of Object.entries(changes)) {
            if (value === '' || (key === 'page' && value <= 1)) next.delete(key)
            else next.set(key, value)
        }
        setParams(next)
    }

    function setPage(newPage) {
        updateParams({ page: newPage })
    }

    const [animes, setAnimes] = useState(null) // null until the first search finishes
    const [totalCount, setTotalCount] = useState(0)
    const [totalPages, setTotalPages] = useState(1)
    const [genres, setGenres] = useState([])
    const [studios, setStudios] = useState([])
    const [error, setError] = useState('')
    const [filterError, setFilterError] = useState('') // kept apart so a successful search doesn't hide it

    useEffect(() => {
        getAnimeGenres().then(setGenres).catch(err => setFilterError(err.message))
        getStudios().then(setStudios).catch(err => setFilterError(err.message))
    }, [])

    useEffect(() => {
        let ignore = false

        searchAnime({ search, author, genre, studioId, page, pageSize: PAGE_SIZE })
            .then(result => {
                if (ignore) return
                // A page past the end (old link, deleted anime, typed URL): jump to the last real page
                if (page > 1 && page > result.totalPages) {
                    setParams(prev => {
                        const next = new URLSearchParams(prev)
                        if (result.totalPages > 1) next.set('page', result.totalPages)
                        else next.delete('page')
                        return next
                    }, { replace: true })
                    return
                }
                setAnimes(result.items)
                setTotalCount(result.totalCount)
                setTotalPages(Math.max(result.totalPages, 1))
                setError('')
            })
            .catch(err => { if (!ignore) setError(err.message) })

        return () => { ignore = true }
    }, [search, author, genre, studioId, page, reloadKey, setParams])

    function applySearch(e) {
        e.preventDefault()
        updateParams({ search: searchInput.trim(), author: authorInput.trim(), page: 1 })
    }

    function clearFilters() {
        setSearchInput('')
        setAuthorInput('')
        setParams({})
    }

    async function handleDelete(anime) {
        if (!window.confirm(`Delete "${anime.name}"? This can't be undone.`)) return

        try {
            await deleteAnime(anime.id)
            if (animes.length === 1 && page > 1) {
                setPage(page - 1)
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

            <form className="search-form" onSubmit={applySearch}>
                <input
                    className="search-bar"
                    type="search"
                    placeholder="Search anime by title..."
                    value={searchInput}
                    onChange={e => setSearchInput(e.target.value)}
                />
                <button type="submit" className="primary">Search</button>
            </form>

            <div className="filters">
                <input
                    placeholder="Author"
                    value={authorInput}
                    onChange={e => setAuthorInput(e.target.value)}
                    onKeyDown={e => { if (e.key === 'Enter') applySearch(e) }}
                />

                <select value={genre} onChange={e => updateParams({ genre: e.target.value, page: 1 })}>
                    <option value="">All genres</option>
                    {genres.map(g => (
                        <option key={g} value={g}>{g}</option>
                    ))}
                </select>

                <select value={studioId} onChange={e => updateParams({ studio: e.target.value, page: 1 })}>
                    <option value="">All studios</option>
                    {studios.map(s => (
                        <option key={s.studioId} value={s.studioId}>{s.nameS}</option>
                    ))}
                </select>

                <button onClick={clearFilters}>Clear filters</button>
            </div>

            {filterError && filterError !== error && <p className="error">{filterError}</p>}
            {error && <p className="error">{error}</p>}
            {animes ? <p>{totalCount} anime found</p> : !error && <p>Loading anime...</p>}

            {!animes && !error && <SkeletonCards count={PAGE_SIZE} lines={4} />}

            <div className="grid">
                {animes?.map(a => (
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
                <button disabled={page <= 1} onClick={() => setPage(page - 1)}>
                    ← Previous
                </button>
                <span>Page {page} of {totalPages}</span>
                <button disabled={page >= totalPages} onClick={() => setPage(page + 1)}>
                    Next →
                </button>
            </div>
        </main>
    )
}

export default Home
