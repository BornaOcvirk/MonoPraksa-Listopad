import { Link, useParams } from 'react-router-dom'
import { getManga, getStudio, searchAnime } from './api'
import useLoad from './useLoad'
import Fields from './Fields'
import LinkList from './LinkList'
import { SkeletonDetails } from './Skeleton'

// Manga info plus its studio and anime adaptations
async function loadManga(id) {
    const manga = await getManga(id)
    const [studio, animes] = await Promise.all([
        getStudio(manga.studioId).catch(() => null), // a missing studio shouldn't hide the manga
        searchAnime({ mangaId: id, pageSize: 100 }),
    ])
    return { manga, studio, animes: animes?.items ?? [] }
}

function MangaDetails() {
    const { id } = useParams()
    const { data, error } = useLoad(() => loadManga(id), id)

    return (
        <main>
            <Link to="/mangas">← Back to mangas</Link>
            {error && <p className="error">{error}</p>}
            {!data && !error && <SkeletonDetails rows={5} />}

            {data && (
                <article className="details">
                    <h1>{data.manga.title}</h1>
                    <Fields fields={[
                        ['Author', data.manga.author],
                        ['Volumes', data.manga.volumes],
                        ['Popularity', data.manga.popularity],
                        ['Studio', data.studio && <Link to={`/studios/${data.studio.studioId}`}>{data.studio.nameS}</Link>],
                    ]} />
                    <LinkList title="Anime adaptations" items={data.animes.map(a => ({ id: a.id, to: `/anime/${a.id}`, label: a.name }))} />
                </article>
            )}
        </main>
    )
}

export default MangaDetails
