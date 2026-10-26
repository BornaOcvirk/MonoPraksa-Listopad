import { useEffect, useState } from 'react'
import { getStudios, getMangas, getAnimeGenres } from './api'

const POPULARITY_OPTIONS = ['Very high', 'High', 'Medium', 'Low']

const EMPTY_ANIME = {
    name: '',
    genre: '',
    numberOfSeasons: 1,
    price: '',
    releaseDate: '',
    studioId: '',
    mangaId: '',
    popularity: '',
}

function AnimeForm({ initialValues = EMPTY_ANIME, submitLabel, onSubmit, onCancel }) {
    const [values, setValues] = useState(initialValues)
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

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')
        setSaving(true)
        try {
            await onSubmit({
                name: values.name.trim(),
                genre: values.genre.trim(),
                numberOfSeasons: Number(values.numberOfSeasons),
                price: Number(values.price),
                releaseDate: values.releaseDate,
                studioId: values.studioId,
                mangaId: values.mangaId || null,
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
                <input name="name" value={values.name} onChange={handleChange} required />
            </label>

            <label>
                Genre
                <input name="genre" value={values.genre} onChange={handleChange} list="genre-list" required />
                <datalist id="genre-list">
                    {genres.map(g => <option key={g} value={g} />)}
                </datalist>
            </label>

            <label>
                Number of seasons
                <input name="numberOfSeasons" type="number" min="0" value={values.numberOfSeasons} onChange={handleChange} required />
            </label>

            <label>
                Price
                <input name="price" type="number" min="0.01" step="0.01" value={values.price} onChange={handleChange} required />
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
                </select>
            </label>

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
