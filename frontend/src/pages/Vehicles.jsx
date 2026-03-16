import { useState, useEffect } from 'react';
import Navbar from '../components/Navbar';
import { getVehicles, createVehicle } from '../api/api';

function Vehicles() {
  const [vehicles, setVehicles] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const [form, setForm] = useState({
    ownerName: '',
    carModel: '',
    batteryCapacityKwh: '',
    connectorType: 'Type2',
    currentBatteryPercent: '',
  });

  useEffect(() => {
    loadVehicles();
  }, []);

  const loadVehicles = async () => {
    try {
      const response = await getVehicles();
      setVehicles(response.data);
    } catch (err) {
      console.error('Failed to load vehicles', err);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');
    try {
      await createVehicle({
        ...form,
        batteryCapacityKwh: parseFloat(form.batteryCapacityKwh),
        currentBatteryPercent: parseFloat(form.currentBatteryPercent),
      });
      setSuccess('Vehicle added successfully!');
      setForm({
        ownerName: '',
        carModel: '',
        batteryCapacityKwh: '',
        connectorType: 'Type2',
        currentBatteryPercent: '',
      });
      setShowForm(false);
      loadVehicles();
    } catch (err) {
      setError(err.response?.data || 'Failed to add vehicle.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <Navbar />
      <div style={styles.content}>
        <div style={styles.header}>
          <h2 style={styles.title}>🚗 My Vehicles</h2>
          <button
            style={styles.addBtn}
            onClick={() => setShowForm(!showForm)}
          >
            {showForm ? 'Cancel' : '+ Add Vehicle'}
          </button>
        </div>

        {/* Add Vehicle Form */}
        {showForm && (
          <div style={styles.card}>
            <h3 style={styles.cardTitle}>Add New Vehicle</h3>
            {error && <div style={styles.error}>{error}</div>}

            <form onSubmit={handleSubmit}>
              <div style={styles.grid}>
                <div style={styles.field}>
                  <label style={styles.label}>Owner Name</label>
                  <input
                    style={styles.input}
                    type="text"
                    placeholder="Geet"
                    value={form.ownerName}
                    onChange={(e) => setForm({ ...form, ownerName: e.target.value })}
                    required
                  />
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Car Model</label>
                  <input
                    style={styles.input}
                    type="text"
                    placeholder="Tesla Model 3"
                    value={form.carModel}
                    onChange={(e) => setForm({ ...form, carModel: e.target.value })}
                    required
                  />
                </div>

                <div style={styles.field}>
                  <label style={styles.label}>Battery Capacity (kWh)</label>
                  <input
                    style={styles.input}
                    type="number"
                    placeholder="75"
                    value={form.batteryCapacityKwh}
                    onChange={(e) => setForm({ ...form, batteryCapacityKwh: e.target.value })}
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
                  <label style={styles.label}>Current Battery (%)</label>
                  <input
                    style={styles.input}
                    type="number"
                    min="0" max="100"
                    placeholder="20"
                    value={form.currentBatteryPercent}
                    onChange={(e) => setForm({ ...form, currentBatteryPercent: e.target.value })}
                    required
                  />
                </div>
              </div>

              <button style={styles.submitBtn} type="submit" disabled={loading}>
                {loading ? 'Adding...' : 'Add Vehicle'}
              </button>
            </form>
          </div>
        )}

        {/* Success Message */}
        {success && <div style={styles.success}>{success}</div>}

        {/* Vehicles List */}
        {vehicles.length > 0 ? (
          <div style={styles.grid}>
            {vehicles.map(vehicle => (
              <div key={vehicle.id} style={styles.vehicleCard}>
                <div style={styles.vehicleIcon}>🚗</div>
                <h3 style={styles.vehicleName}>{vehicle.carModel}</h3>
                <div style={styles.vehicleDetails}>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Owner</span>
                    <span style={styles.detailValue}>{vehicle.ownerName}</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Battery</span>
                    <span style={styles.detailValue}>{vehicle.batteryCapacityKwh} kWh</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Connector</span>
                    <span style={styles.detailValue}>{vehicle.connectorType}</span>
                  </div>
                  <div style={styles.detailRow}>
                    <span style={styles.detailLabel}>Current Charge</span>
                    <span style={styles.detailValue}>{vehicle.currentBatteryPercent}%</span>
                  </div>
                </div>
                <div style={styles.vehicleId}>ID: #{vehicle.id}</div>
              </div>
            ))}
          </div>
        ) : (
          <div style={styles.emptyState}>
            <p>No vehicles added yet.</p>
            <p>Click <strong>"+ Add Vehicle"</strong> to add your first vehicle!</p>
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
  vehicleCard: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '20px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
    textAlign: 'center',
  },
  vehicleIcon: { fontSize: '48px', marginBottom: '8px' },
  vehicleName: { margin: '0 0 16px 0', color: '#333' },
  vehicleDetails: { textAlign: 'left' },
  detailRow: {
    display: 'flex',
    justifyContent: 'space-between',
    padding: '6px 0',
    borderBottom: '1px solid #f0f0f0',
  },
  detailLabel: { color: '#888', fontSize: '13px' },
  detailValue: { color: '#333', fontSize: '13px', fontWeight: 'bold' },
  vehicleId: { marginTop: '12px', color: '#aaa', fontSize: '12px' },
  emptyState: {
    textAlign: 'center',
    padding: '60px',
    color: '#888',
    backgroundColor: 'white',
    borderRadius: '12px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
  },
};

export default Vehicles;



