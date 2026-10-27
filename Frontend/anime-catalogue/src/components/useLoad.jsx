import { useEffect, useState } from 'react'

// Runs an async loader whenever `key` changes. Results for an old key are ignored,
// so a slow response can't overwrite a newer page. Returns { data, error, setData }.
export default function useLoad(loader, key = '') {
    const [state, setState] = useState({ key: null, data: null, error: '' })

    useEffect(() => {
        let ignore = false
        loader()
            .then(data => { if (!ignore) setState({ key, data, error: '' }) })
            .catch(err => { if (!ignore) setState({ key, data: null, error: err.message }) })
        return () => { ignore = true }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [key])

    const current = state.key === key
    return {
        data: current ? state.data : null,
        error: current ? state.error : '',
        setData: data => setState(s => ({ ...s, data: typeof data === 'function' ? data(s.data) : data })),
    }
}
