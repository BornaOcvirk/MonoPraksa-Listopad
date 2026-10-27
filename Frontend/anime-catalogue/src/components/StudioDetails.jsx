import { Link, useParams } from 'react-router-dom'
import { getStudio, getMangasByStudio, searchAnime } from './api'
import useLoad from './useLoad'
import Fields from './Fields'
import LinkList from './LinkList'

// Studio info plus the anime and manga linked to it
async function loadStudio(id) {
    const [studio, mangas, animes] = await Promise.all([
        getStudio(id),
        getMangasByStudio(id),
        searchAnime({ studioId: id, pageSize: 100 }),
    ])
    return { studio, mangas: mangas ?? [], animes: animes?.items ?? [] }
}

function StudioDetails() {
    const { id } = useParams()
    const { data, error } = useLoad(() => loadStudio(id), id)

    return (
        <main>
            <Link to="/studios">← Back to studios</Link>
            {error && <p className="error">{error}</p>}
            {!data && !error && <p>Loading...</p>}

            {data && (
                <article className="details">
                    <h1>{data.studio.nameS}</h1>
                    <Fields fields={[
                        ['Country', data.studio.country],
                        ['Established', data.studio.established],
                    ]} />
                    <LinkList title="Anime" items={data.animes.map(a => ({ id: a.id, to: `/anime/${a.id}`, label: a.name }))} />
                    <LinkList title="Manga" items={data.mangas.map(m => ({ id: m.mangaId, to: `/mangas/${m.mangaId}`, label: m.title }))} />
                </article>
            )}
        </main>
    )
}

export default StudioDetails
