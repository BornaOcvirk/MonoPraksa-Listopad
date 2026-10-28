import { Link } from 'react-router-dom'
import { getMangas } from './api'
import useLoad from './useLoad'
import { SkeletonCards } from './Skeleton'

function MangaList() {
    const { data: mangas, error } = useLoad(getMangas)

    return (
        <main>
            <h2>Mangas</h2>
            {error && <p className="error">{error}</p>}
            {!mangas && !error && <SkeletonCards lines={3} />}
            {mangas?.length === 0 && <p>No mangas yet.</p>}

            <div className="grid">
                {mangas?.map(m => (
                    <article className="card" key={m.mangaId}>
                        <h3><Link to={`/mangas/${m.mangaId}`}>{m.title}</Link></h3>
                        <p>By {m.author}</p>
                        <p>{m.volumes} volume(s)</p>
                        <p>Popularity: {m.popularity || 'Not set'}</p>
                    </article>
                ))}
            </div>
        </main>
    )
}

export default MangaList
