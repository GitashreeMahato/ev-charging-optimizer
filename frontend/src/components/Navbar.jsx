import { useNavigate, Link } from 'react-router-dom';

function Navbar() {
  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem('user') || '{}');

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    navigate('/login');
  };

  return (
    <div style={styles.navbar}>
      <div style={styles.navLeft}>
        <h1 style={styles.navTitle}>⚡ EV Charging Optimizer</h1>
        <nav style={styles.navLinks}>
          <Link to="/dashboard" style={styles.navLink}>Dashboard</Link>
          <Link to="/vehicles" style={styles.navLink}>My Vehicles</Link>
          <Link to="/stations" style={styles.navLink}>My Stations</Link>
        </nav>
      </div>
      <div style={styles.navRight}>
        <span style={styles.navUser}>👤 {user.fullName}</span>
        <button style={styles.logoutBtn} onClick={handleLogout}>Logout</button>
      </div>
    </div>
  );
}

const styles = {
  navbar: {
    backgroundColor: '#2d7a4f',
    padding: '16px 24px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  navLeft: {
    display: 'flex',
    alignItems: 'center',
    gap: '32px',
  },
  navTitle: {
    color: 'white',
    margin: 0,
    fontSize: '20px',
  },
  navLinks: {
    display: 'flex',
    gap: '20px',
  },
  navLink: {
    color: 'white',
    textDecoration: 'none',
    fontSize: '14px',
    opacity: 0.9,
    padding: '4px 8px',
    borderRadius: '4px',
  },
  navRight: {
    display: 'flex',
    alignItems: 'center',
    gap: '16px',
  },
  navUser: {
    color: 'white',
    fontSize: '14px',
  },
  logoutBtn: {
    backgroundColor: 'transparent',
    color: 'white',
    border: '1px solid white',
    padding: '6px 12px',
    borderRadius: '6px',
    cursor: 'pointer',
    fontSize: '14px',
  },
};

export default Navbar;
