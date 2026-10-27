import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import AnimeForm from './AnimeForm'
import { getAnime, updateAnime } from './api'

function EditAnime() {
    const { id } = useParams()
    const navigate = useNavigate()
    const [anime, setAnime] = useState(null)
    const [error, setError] = useState('')

    useEffect(() => {
        getAnime(id)
            .then(a => setAnime({
                name: a.name,
                genre: a.genre,
                numberOfSeasons: a.numberOfSeasons,
                releaseDate: a.releaseDate,
                studioId: a.studioId,
                mangaId: a.mangaId ?? '',
                popularity: a.popularity ?? '',
            }))
            .catch(err => setError(err.message))
    }, [id])

    async function handleUpdate(values) {
        await updateAnime(id, values)
        navigate('/')
    }

    return (
        <main>
            <h1>Edit anime</h1>

            {error && <p className="error">{error}</p>}
            {!anime && !error && <p>Loading...</p>}

            {anime && (
                <AnimeForm
                    initialValues={anime}
                    submitLabel="Save changes"
                    onSubmit={handleUpdate}
                    onCancel={() => navigate('/')}
                />
            )}
        </main>
    )
}

export default EditAnime
