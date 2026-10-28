import { useEffect, useState } from 'react'
import { getStudios, getMangas, getAnimeGenres } from './api'

const POPULARITY_OPTIONS = ['Very high', 'High', 'Medium', 'Low']

// Value of the "+ Add new manga..." option in the manga dropdown
const NEW_MANGA = 'new'
const EMPTY_MANGA = { title: '', author: '', volumes: 1, popularity: '' }

const EMPTY_ANIME = {
    name: '',
    genre: '',
    numberOfSeasons: 1,
    releaseDate: '',
    studioId: '',
    mangaId: '',
    popularity: '',
}

function AnimeForm({ initialValues = EMPTY_ANIME, submitLabel, onSubmit, onCancel }) {
    const [values, setValues] = useState(initialValues)
    const [newManga, setNewManga] = useState(EMPTY_MANGA)
    const [studios, setStudios] = useState([])
    const [mangas, setMangas] = useState([])
    const [genres, setGenres] = useState([])
    const [error, setError] = useState('')
    const [saving, setSaving] = useState(false)

    useEffect(() => {
        getStudios().then(setStudios).catch(err => setError(err.message))
        getMangas().then(setMangas).catch(err => setError(err.message))
        getAnimeGenres().then(setGenres).catch(err => setError(err.message))
    }, [])

    function handleChange(e) {
        const { name, value } = e.target
        setValues(v => ({ ...v, [name]: value }))
    }

    function handleMangaChange(e) {
        const { name, value } = e.target
        setNewManga(m => ({ ...m, [name]: value }))
    }

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')
        setSaving(true)
        try {
            const addingManga = values.mangaId === NEW_MANGA

            // A new manga is sent with the anime and saved in the same request (it belongs to the
            // anime's studio), so a failed save never leaves a manga without its anime
            await onSubmit({
                name: values.name.trim(),
                genre: values.genre.trim(),
                numberOfSeasons: Number(values.numberOfSeasons),
                releaseDate: values.releaseDate,
                studioId: values.studioId,
                mangaId: addingManga ? null : values.mangaId || null,
                newManga: addingManga ? {
                    title: newManga.title.trim() || values.name.trim(),
                    author: newManga.author.trim(),
                    volumes: Number(newManga.volumes),
                    popularity: newManga.popularity || null,
                } : null,
                popularity: values.popularity || null,
            })
        } catch (err) {
            setError(err.message)
            setSaving(false)
        }
    }

    return (
        <form className="anime-form" onSubmit={handleSubmit}>
            <label>
                Title
                <input name="name" value={values.name} onChange={handleChange} maxLength={200} required />
            </label>

            <label>
                Genre
                <input name="genre" value={values.genre} onChange={handleChange} list="genre-list" maxLength={100} required />
                <datalist id="genre-list">
                    {genres.map(g => <option key={g} value={g} />)}
                </datalist>
            </label>

            <label>
                Number of seasons
                <input name="numberOfSeasons" type="number" min="0" value={values.numberOfSeasons} onChange={handleChange} required />
            </label>

            <label>
                Release date
                <input name="releaseDate" type="date" value={values.releaseDate} onChange={handleChange} required />
            </label>

            <label>
                Studio
                <select name="studioId" value={values.studioId} onChange={handleChange} required>
                    <option value="">-- choose a studio --</option>
                    {studios.map(s => (
                        <option key={s.studioId} value={s.studioId}>{s.nameS}</option>
                    ))}
                </select>
            </label>

            <label>
                Based on manga
                <select name="mangaId" value={values.mangaId} onChange={handleChange}>
                    <option value="">None (original anime)</option>
                    {mangas.map(m => (
                        <option key={m.mangaId} value={m.mangaId}>{m.title} ({m.author})</option>
                    ))}
                    <option value={NEW_MANGA}>+ Add new manga...</option>
                </select>
            </label>

            {values.mangaId === NEW_MANGA && (
                <fieldset className="new-manga">
                    <legend>New manga</legend>
                    <label>
                        Manga title
                        <input name="title" value={newManga.title} onChange={handleMangaChange} placeholder={values.name || 'Same as anime title'} maxLength={255} />
                    </label>
                    <label>
                        Author
                        <input name="author" value={newManga.author} onChange={handleMangaChange} maxLength={255} required />
                    </label>
                    <label>
                        Volumes
                        <input name="volumes" type="number" min="0" value={newManga.volumes} onChange={handleMangaChange} required />
                    </label>
                    <label>
                        Manga popularity
                        <select name="popularity" value={newManga.popularity} onChange={handleMangaChange}>
                            <option value="">Not set</option>
                            {POPULARITY_OPTIONS.map(p => <option key={p} value={p}>{p}</option>)}
                        </select>
                    </label>
                    <p className="hint">The manga is saved under the studio chosen above. Leave the title empty to use the anime title.</p>
                </fieldset>
            )}

            <label>
                Popularity
                <select name="popularity" value={values.popularity} onChange={handleChange}>
                    <option value="">Not set</option>
                    {POPULARITY_OPTIONS.map(p => <option key={p} value={p}>{p}</option>)}
                </select>
            </label>

            {error && <p className="error">{error}</p>}

            <div className="form-buttons">
                <button type="submit" disabled={saving}>
                    {saving ? 'Saving...' : submitLabel}
                </button>
                <button type="button" onClick={onCancel}>Cancel</button>
            </div>
        </form>
    )
}

export default AnimeForm
