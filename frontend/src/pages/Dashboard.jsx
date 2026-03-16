import { useState, useEffect } from 'react';

import Navbar from '../components/Navbar';
import { optimize, getPrices, getSessions, getVehicles, getStations } from '../api/api';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

function Dashboard() {
  // const navigate = useNavigate();
  // const user = JSON.parse(localStorage.getItem('user') || '{}');

  const [vehicles, setVehicles] = useState([]);
  const [stations, setStations] = useState([]);
  const [prices, setPrices] = useState([]);
  const [sessions, setSessions] = useState([]);
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const [form, setForm] = useState({
    vehicleId: '',
    stationId: '',
    currentBatteryPercent: 20,
    targetBatteryPercent: 80,
    batteryCapacityKwh: 75,
    chargerPowerKw: 11,
    deadLine: '',
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [v, s, p, sess] = await Promise.all([
        getVehicles(),
        getStations(),
        getPrices(),
        getSessions(),
      ]);
      setVehicles(v.data);
      setStations(s.data);
      setSessions(sess.data);

      // Filter today's prices and format for chart
      const today = new Date().toISOString().split('T')[0];
      const todayPrices = p.data
        .filter(price => price.startTimeUtc.startsWith(today))
        .map(price => ({
          time: new Date(price.startTimeUtc).toLocaleTimeString('de-DE', {
            hour: '2-digit',
            minute: '2-digit'
          }),
          price: parseFloat((price.pricePerKwh * 100).toFixed(2))
        }));
      setPrices(todayPrices);
    } catch (err) {
      console.error('Failed to load data', err);
    }
  };

  const handleOptimize = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setResult(null);
    try {
      const response = await optimize({
        ...form,
        vehicleId: parseInt(form.vehicleId),
        stationId: parseInt(form.stationId),
        currentBatteryPercent: parseFloat(form.currentBatteryPercent),
        targetBatteryPercent: parseFloat(form.targetBatteryPercent),
        batteryCapacityKwh: parseFloat(form.batteryCapacityKwh),
        chargerPowerKw: parseFloat(form.chargerPowerKw),
      });
      setResult(response.data);
      loadData(); // Refresh sessions
    } catch (err) {
      setError(err.response?.data || 'Optimization failed.');
    } finally {
      setLoading(false);
    }
  };

  // const handleLogout = () => {
  //   localStorage.removeItem('token');
  //   localStorage.removeItem('user');
  //   navigate('/login');
  // };

  return (
        <div style={styles.container}>
      <Navbar />


      <div style={styles.content}>

        {/* Price Chart */}
        <div style={styles.card}>
          <h2 style={styles.cardTitle}>📈 Today's Electricity Prices (ct/kWh)</h2>
          {prices.length > 0 ? (
            <ResponsiveContainer width="100%" height={250}>
              <LineChart data={prices}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="time" tick={{ fontSize: 10 }} interval={7} />
                <YAxis tick={{ fontSize: 10 }} />
                <Tooltip formatter={(value) => [`${value} ct/kWh`, 'Price']} />
                <Line
                  type="monotone"
                  dataKey="price"
                  stroke="#2d7a4f"
                  dot={false}
                  strokeWidth={2}
                />
              </LineChart>
            </ResponsiveContainer>
          ) : (
            <p style={styles.noData}>No price data available for today.</p>
          )}
        </div>

        {/* Optimizer Form */}
        <div style={styles.card}>
          <h2 style={styles.cardTitle}>🔋 Optimize Charging</h2>

          {error && <div style={styles.error}>{error}</div>}

          <form onSubmit={handleOptimize}>
            <div style={styles.grid}>
              <div style={styles.field}>
                <label style={styles.label}>Vehicle</label>
                <select
                  style={styles.input}
                  value={form.vehicleId}
                  onChange={(e) => setForm({ ...form, vehicleId: e.target.value })}
                  required
                >
                  <option value="">Select vehicle</option>
                  {vehicles.map(v => (
                    <option key={v.id} value={v.id}>{v.carModel}</option>
                  ))}
                </select>
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Charging Station</label>
                <select
                  style={styles.input}
                  value={form.stationId}
                  onChange={(e) => setForm({ ...form, stationId: e.target.value })}
                  required
                >
                  <option value="">Select station</option>
                  {stations.map(s => (
                    <option key={s.id} value={s.id}>{s.name}</option>
                  ))}
                </select>
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Current Battery (%)</label>
                <input
                  style={styles.input}
                  type="number"
                  min="0" max="100"
                  value={form.currentBatteryPercent}
                  onChange={(e) => setForm({ ...form, currentBatteryPercent: e.target.value })}
                  required
                />
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Target Battery (%)</label>
                <input
                  style={styles.input}
                  type="number"
                  min="0" max="100"
                  value={form.targetBatteryPercent}
                  onChange={(e) => setForm({ ...form, targetBatteryPercent: e.target.value })}
                  required
                />
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Battery Capacity (kWh)</label>
                <input
                  style={styles.input}
                  type="number"
                  value={form.batteryCapacityKwh}
                  onChange={(e) => setForm({ ...form, batteryCapacityKwh: e.target.value })}
                  required
                />
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Charger Power (kW)</label>
                <input
                  style={styles.input}
                  type="number"
                  value={form.chargerPowerKw}
                  onChange={(e) => setForm({ ...form, chargerPowerKw: e.target.value })}
                  required
                />
              </div>

              <div style={styles.field}>
                <label style={styles.label}>Deadline</label>
                <input
                  style={styles.input}
                  type="datetime-local"
                  value={form.deadLine}
                  onChange={(e) => setForm({ ...form, deadLine: e.target.value })}
                  required
                />
              </div>
            </div>

            <button style={styles.button} type="submit" disabled={loading}>
              {loading ? 'Optimizing...' : '⚡ Find Cheapest Window'}
            </button>
          </form>

          {/* Result */}
          {result && (
            <div style={styles.result}>
              <h3 style={styles.resultTitle}>✅ Optimal Charging Window Found!</h3>
              <div style={styles.resultGrid}>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Cheapest Window</span>
                  <span style={styles.resultValue}>{result.cheapestWindow}</span>
                </div>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Energy Needed</span>
                  <span style={styles.resultValue}>{result.energyNeededKwh} kWh</span>
                </div>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Duration</span>
                  <span style={styles.resultValue}>{result.chargingDurationHours} hrs</span>
                </div>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Estimated Cost</span>
                  <span style={styles.resultValue}>€{result.estimatedCostEur}</span>
                </div>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Avg Price/kWh</span>
                  <span style={styles.resultValue}>€{result.averagePricePerKwh}</span>
                </div>
                <div style={styles.resultItem}>
                  <span style={styles.resultLabel}>Session ID</span>
                  <span style={styles.resultValue}>#{result.sessionId}</span>
                </div>
              </div>
            </div>
          )}
        </div>

        {/* Session History */}
        <div style={styles.card}>
          <h2 style={styles.cardTitle}>📋 Charging Session History</h2>
          {sessions.length > 0 ? (
            <table style={styles.table}>
              <thead>
                <tr style={styles.tableHeader}>
                  <th style={styles.th}>ID</th>
                  <th style={styles.th}>Start Time</th>
                  <th style={styles.th}>End Time</th>
                  <th style={styles.th}>Energy (kWh)</th>
                  <th style={styles.th}>Cost (€)</th>
                </tr>
              </thead>
              <tbody>
                {sessions.map(session => (
                  <tr key={session.id} style={styles.tableRow}>
                    <td style={styles.td}>#{session.id}</td>
                    <td style={styles.td}>
                      {new Date(session.startTime).toLocaleString('de-DE')}
                    </td>
                    <td style={styles.td}>
                      {new Date(session.endTime).toLocaleString('de-DE')}
                    </td>
                    <td style={styles.td}>{session.energyDeliveredKwh}</td>
                    <td style={styles.td}>€{session.totalCostEur}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <p style={styles.noData}>No charging sessions yet.</p>
          )}
        </div>

      </div>
    </div>
  );
}

const styles = {
  container: { minHeight: '100vh', backgroundColor: '#f0f4f8' },
  navbar: {
    backgroundColor: '#2d7a4f',
    padding: '16px 24px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  navTitle: { color: 'white', margin: 0, fontSize: '20px' },
  navRight: { display: 'flex', alignItems: 'center', gap: '16px' },
  navUser: { color: 'white', fontSize: '14px' },
  logoutBtn: {
    backgroundColor: 'transparent',
    color: 'white',
    border: '1px solid white',
    padding: '6px 12px',
    borderRadius: '6px',
    cursor: 'pointer',
    fontSize: '14px',
  },
  content: { padding: '24px', maxWidth: '1200px', margin: '0 auto' },
  card: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '24px',
    marginBottom: '24px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
  },
  cardTitle: { margin: '0 0 16px 0', color: '#333', fontSize: '18px' },
  grid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
    gap: '16px',
    marginBottom: '16px',
  },
  field: { display: 'flex', flexDirection: 'column' },
  label: { marginBottom: '6px', color: '#555', fontWeight: 'bold', fontSize: '13px' },
  input: {
    padding: '8px',
    borderRadius: '6px',
    border: '1px solid #ddd',
    fontSize: '14px',
  },
  button: {
    padding: '12px 24px',
    backgroundColor: '#2d7a4f',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    fontSize: '16px',
    cursor: 'pointer',
  },
  error: {
    backgroundColor: '#ffe0e0',
    color: '#c0392b',
    padding: '10px',
    borderRadius: '8px',
    marginBottom: '16px',
    fontSize: '14px',
  },
  result: {
    backgroundColor: '#e8f5e9',
    borderRadius: '8px',
    padding: '16px',
    marginTop: '16px',
  },
  resultTitle: { margin: '0 0 12px 0', color: '#2d7a4f' },
  resultGrid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(150px, 1fr))',
    gap: '12px',
  },
  resultItem: { display: 'flex', flexDirection: 'column' },
  resultLabel: { fontSize: '12px', color: '#666' },
  resultValue: { fontSize: '16px', fontWeight: 'bold', color: '#2d7a4f' },
  table: { width: '100%', borderCollapse: 'collapse' },
  tableHeader: { backgroundColor: '#f5f5f5' },
  th: { padding: '10px', textAlign: 'left', fontSize: '13px', color: '#555', borderBottom: '2px solid #eee' },
  tableRow: { borderBottom: '1px solid #eee' },
  td: { padding: '10px', fontSize: '14px', color: '#333' },
  noData: { color: '#999', textAlign: 'center', padding: '20px' },
};

export default Dashboard;
