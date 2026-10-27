import { Link } from 'react-router-dom'

// Titled list of links, used on the studio and manga detail pages.
// items: [{ id, to, label }]
function LinkList({ title, items }) {
    return (
        <section>
            <h2>{title}</h2>
            {items.length === 0 ? <p>None yet.</p> : (
                <ul className="link-list">
                    {items.map(i => <li key={i.id}><Link to={i.to}>{i.label}</Link></li>)}
                </ul>
            )}
        </section>
    )
}

export default LinkList
