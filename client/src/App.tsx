
import { useState, useEffect } from "react"

function App() {
  const [activities, setActivities] = useState([]);

  useEffect(() => {
    fetch('https://localhost:5001/api/activities')
      .then(response => response.json())
      .then(data => setActivities(data))
  }, [])

  return (
    <div>
      <h3 className="app" style={{color: 'red'}}>Reactivities</h3>
      <ul>
        {activities.map((activity) => (
          <li key={activity.id}>{activity.title}</li>
        ))}
      </ul>
    </div>
  )
}

export default App
