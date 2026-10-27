import { Fragment } from 'react'

// Label/value pairs as a definition list. fields: [[label, value], ...]
function Fields({ fields }) {
    return (
        <dl>
            {fields.map(([label, value]) => (
                <Fragment key={label}>
                    <dt>{label}</dt>
                    <dd>{value || value === 0 ? value : 'Not set'}</dd>
                </Fragment>
            ))}
        </dl>
    )
}

export default Fields
