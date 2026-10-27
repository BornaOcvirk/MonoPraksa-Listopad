import { useNavigate } from 'react-router-dom'
import AnimeForm from './AnimeForm'
import { createAnime } from './api'

function AddAnime() {
    const navigate = useNavigate()

    async function handleCreate(anime) {
        await createAnime(anime)
        navigate('/')
    }

    return (
        <main>
            <h1>Add anime</h1>
            <AnimeForm
                submitLabel="Add anime"
                onSubmit={handleCreate}
                onCancel={() => navigate('/')}
            />
        </main>
    )
}

export default AddAnime
