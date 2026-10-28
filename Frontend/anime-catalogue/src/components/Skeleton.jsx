// "Ghost" placeholders shown while a page loads: empty boxes with softly pulsing bars.
// Screen readers hear "Loading..." once instead of the empty shapes.

function Bar({ width = '100%', kind = '' }) {
    return <span className={`ghost-bar ${kind}`} style={{ width }} />
}

function Loading({ className, children }) {
    return (
        <div className={className} role="status">
            <span className="visually-hidden">Loading...</span>
            {children}
        </div>
    )
}

// A grid of empty cards (catalogue, studios, mangas). The pulse ripples from card to card.
export function SkeletonCards({ count = 6, lines = 3 }) {
    return (
        <Loading className="grid">
            {Array.from({ length: count }, (_, i) => (
                <div className="card ghost" key={i} aria-hidden="true" style={{ '--delay': `${i * 0.12}s` }}>
                    <Bar width="70%" kind="title" />
                    {Array.from({ length: lines }, (_, j) => <Bar key={j} width={`${90 - j * 15}%`} />)}
                </div>
            ))}
        </Loading>
    )
}

// An empty details box: a title and label/value rows (anime, studio, manga, profile)
export function SkeletonDetails({ rows = 4 }) {
    return (
        <Loading className="details ghost">
            <div aria-hidden="true">
                <Bar width="55%" kind="title" />
                {Array.from({ length: rows }, (_, i) => (
                    <div className="ghost-row" key={i}>
                        <Bar />
                        <Bar width={`${70 - (i % 3) * 15}%`} />
                    </div>
                ))}
            </div>
        </Loading>
    )
}

// A table with real headers and empty rows (users)
export function SkeletonTable({ headers, rows = 5 }) {
    return (
        <Loading className="table-wrap">
            <table className="table" aria-hidden="true">
                <thead>
                    <tr>{headers.map(h => <th key={h}>{h}</th>)}</tr>
                </thead>
                <tbody>
                    {Array.from({ length: rows }, (_, i) => (
                        <tr key={i}>
                            {headers.map((h, j) => <td key={h}><Bar width={`${80 - ((i + j) % 3) * 15}%`} /></td>)}
                        </tr>
                    ))}
                </tbody>
            </table>
        </Loading>
    )
}

// An empty form: label + input pairs (edit anime)
export function SkeletonForm({ fields = 7 }) {
    return (
        <Loading className="anime-form ghost">
            {Array.from({ length: fields }, (_, i) => (
                <div key={i} aria-hidden="true">
                    <Bar width="35%" />
                    <Bar kind="field" />
                </div>
            ))}
        </Loading>
    )
}
