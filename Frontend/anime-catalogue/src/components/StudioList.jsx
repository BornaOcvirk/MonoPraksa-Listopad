import { Link } from 'react-router-dom'
import { getStudios } from './api'
import useLoad from './useLoad'

function StudioList() {
    const { data: studios, error } = useLoad(getStudios)

    return (
        <main>
            <h2>Studios</h2>
            {error && <p className="error">{error}</p>}
            {!studios && !error && <p>Loading...</p>}
            {studios?.length === 0 && <p>No studios yet.</p>}

            <div className="grid">
                {studios?.map(s => (
                    <article className="card" key={s.studioId}>
                        <h3><Link to={`/studios/${s.studioId}`}>{s.nameS}</Link></h3>
                        <p>Country: {s.country || 'Unknown'}</p>
                        <p>Established: {s.established?.slice(0, 4) || 'Unknown'}</p>
                    </article>
                ))}
            </div>
        </main>
    )
}

export default StudioList
