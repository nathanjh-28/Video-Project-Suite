import { useState, useEffect } from 'react'
import './App.css'

// we can recreate this test in the console using 
// fetch('/api/test').then(r => r.json()).then(console.log)

function App() {
  const [apiData, setApiData] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const testApiConnection = async () => {
    setLoading(true)
    setError(null)

    try {
      const response = await fetch('/api/test')

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      const data = await response.json()
      setApiData(data)
    } catch (err) {
      setError(err.message)
      console.error('API call failed:', err)
    } finally {
      setLoading(false)
    }
  }

  // Test connection on component mount
  useEffect(() => {
    testApiConnection()
  }, [])

  return (
    <div className="App">
      <h1>Video Project Suite</h1>
      <h2>API Connection Test</h2>

      <button onClick={testApiConnection} disabled={loading}>
        {loading ? 'Testing...' : 'Test API Connection'}
      </button>

      {loading && <p>Loading...</p>}

      {error && (
        <div style={{ color: 'red', marginTop: '10px' }}>
          <strong>Error:</strong> {error}
        </div>
      )}

      {apiData && (
        <div style={{ marginTop: '20px', padding: '10px', background: '#f0f0f0' }}>
          <h3>✅ API Connected Successfully!</h3>
          <p><strong>Message:</strong> {apiData.message}</p>
          <p><strong>Timestamp:</strong> {apiData.timestamp}</p>
        </div>
      )}
    </div>
  )
}

export default App