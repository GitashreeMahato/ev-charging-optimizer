import { useState, useEffect } from 'react';
import Navbar from '../components/Navbar';
import { getStations, createStation } from '../api/api';

function Stations() {
  const [stations, setStations] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const [form, setForm] = useState({
    name: '',
    location: '',
    powerCapacityKw: '',
    connectorType: 'Type2',
    isAvailable: true,
    pricePerKwh: 0,
  });

  useEffect(() => {
    loadStations();
  }, []);

  const loadStations = async () => {
    try {
      const response = await getStations();
      setStations(response.data);
    } catch (err) {
      console.error('Failed to load stations', err);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');
    try {
      await createStation({
        ...form,
        powerCapacityKw: parseFloat(form.powerCapacityKw),
        pricePerKwh: parseFloat(form.pricePerKwh),
      });
      setSuccess('Charging station added successfully!');
      setForm({
        name: '',
        location: '',
        powerCapacityKw: '',
        connectorType: 'Type2',
        isAvailable: true,
        pricePerKwh: 0,
      });
      setShowForm(false);
      loadStations();
    } catch (err) {
      setError(err.response?.data || 'Failed to add station.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <Navbar />
      <div style={styles.content}>
        <div style={styles.header}>
          <h2 style={styles.title}>⚡ My Charging Stations</h2>
          <button
            style={styles.addBtn}
            onClick={() => setShowForm(!showForm)}
          >
            {showForm ? 'Cancel' : '+ Add Station'}
          </button>
        </div>

        {/* Add Station Form */}
        {showForm && (
          <div style={styles.card}>
            <h3 style={styles.cardTitle}>Add New Charging Station</h3>
            {error && <div style={styles.error}>{error}</div>}

            <form onSubmit={handleSubmit}>
              <div style={styles.grid}>
                <div style={styles.field}>
                  <label style={styles.label}>Station Name</label>
                  <input
                    style={styles.input}
                    type="text"
                    placeholder="Home Garage"
                    value={form.name}
                    onChange={(e) => setForm({ ...form, name: e.target.value })}
                    required
                  />
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Location</label>
                  <input
                    style={styles.input}
                    type="text"
                    placeholder="Berlin, Germany"
                    value={form.location}
                    onChange={(e) => setForm({ ...form, location: e.target.value })}
                    required
                  />
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Power Capacity (kW)</label>
                  <input
                    style={styles.input}
                    type="number"
                    placeholder="11"
                    value={form.powerCapacityKw}
                    onChange={(e) => setForm({ ...form, powerCapacityKw: e.target.value })}
                    required
                  />
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Connector Type</label>
                  <select
                    style={styles.input}
                    value={form.connectorType}
                    onChange={(e) => setForm({ ...form, connectorType: e.target.value })}
                  >
                    <option value="Type2">Type 2</option>
                    <option value="CCS">CCS</option>
                    <option value="CHAdeMO">CHAdeMO</option>
                    <option value="Tesla">Tesla</option>
                  </select>
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Available?</label>
                  <select
                    style={styles.input}
                    value={form.isAvailable}
                    onChange={(e) => setForm({ ...form, isAvailable: e.target.value === 'true' })}
                  >
                    <option value="true">Yes</option>
                    <option value="false">No</option>
                  </select>
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Price Per kWh (€)</label>
                  <input
                    style={styles.input}
                    type="number"
                    step="0.01"
                    placeholder="0 = use live spot prices, e.g. 0.45 for public charger"
                    value={form.pricePerKwh}
                    onChange={(e) => setForm({ ...form, pricePerKwh: e.target.value })}
                  />
                </div>
              </div>

              <button style={styles.submitBtn} type="submit" disabled={loading}>
                {loading ? 'Adding...' : 'Add Station'}
              </button>
            </form>
          </div>
        )}

        {/* Success Message */}
        {success && <div style={styles.success}>{success}</div>}

        {/* Stations List */}
        {stations.length > 0 ? (
          <div style={styles.grid}>
            {stations.map(station => (
              <div key={station.id} style={styles.stationCard}>
                <div style={styles.stationIcon}>🔌</div>
                <h3 style={styles.stationName}>{station.name}</h3>
                <div style={styles.stationDetails}>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Location</span>
                    <span style={styles.detailValue}>{station.location}</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Power</span>
                    <span style={styles.detailValue}>{station.powerCapacityKw} kW</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Connector</span>
                    <span style={styles.detailValue}>{station.connectorType}</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Available</span>
                    <span style={{
                      ...styles.detailValue,
                      color: station.isAvailable ? '#2d7a4f' : '#c0392b'
                    }}>
                      {station.isAvailable ? '✅ Yes' : '❌ No'}
                    </span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Price/kWh</span>
                    <span style={styles.detailValue}>€{station.pricePerKwh}</span>
                  </div>
                </div>
                <div style={styles.stationId}>ID: #{station.id}</div>
              </div>
            ))}
          </div>
        ) : (
          <div style={styles.emptyState}>
            <p>No charging stations added yet.</p>
            <p>Click <strong>"+ Add Station"</strong> to add your first station!</p>
          </div>
        )}
      </div>
    </div>
  );
}

const styles = {
  container: { minHeight: '100vh', backgroundColor: '#f0f4f8' },
  content: { padding: '24px', maxWidth: '1200px', margin: '0 auto' },
  header: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '24px',
  },
  title: { margin: 0, color: '#333' },
  addBtn: {
    backgroundColor: '#2d7a4f',
    color: 'white',
    border: 'none',
    padding: '10px 20px',
    borderRadius: '8px',
    cursor: 'pointer',
    fontSize: '14px',
  },
  card: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '24px',
    marginBottom: '24px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
  },
  cardTitle: { margin: '0 0 16px 0', color: '#333' },
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
  submitBtn: {
    backgroundColor: '#2d7a4f',
    color: 'white',
    border: 'none',
    padding: '10px 24px',
    borderRadius: '8px',
    cursor: 'pointer',
    fontSize: '14px',
  },
  error: {
    backgroundColor: '#ffe0e0',
    color: '#c0392b',
    padding: '10px',
    borderRadius: '8px',
    marginBottom: '16px',
    fontSize: '14px',
  },
  success: {
    backgroundColor: '#e8f5e9',
    color: '#2d7a4f',
    padding: '10px',
    borderRadius: '8px',
    marginBottom: '16px',
    fontSize: '14px',
  },
  stationCard: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '20px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
    textAlign: 'center',
  },
  stationIcon: { fontSize: '48px', marginBottom: '8px' },
  stationName: { margin: '0 0 16px 0', color: '#333' },
  stationDetails: { textAlign: 'left' },
  detailRow: {
    display: 'flex',
    justifyContent: 'space-between',
    padding: '6px 0',
    borderBottom: '1px solid #f0f0f0',
  },
  detailLabel: { color: '#888', fontSize: '13px' },
  detailValue: { color: '#333', fontSize: '13px', fontWeight: 'bold' },
  stationId: { marginTop: '12px', color: '#aaa', fontSize: '12px' },
  emptyState: {
    textAlign: 'center',
    padding: '60px',
    color: '#888',
    backgroundColor: 'white',
    borderRadius: '12px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
  },
};
export default Stations;