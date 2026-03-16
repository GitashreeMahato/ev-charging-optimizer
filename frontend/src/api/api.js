import axios from 'axios';

const API_URL = 'http://localhost:5181/api';

const api = axios.create({
    baseURL: API_URL,
});

// Automatically add JWT token to every request
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Auth
export const register = (data) => api.post('/Auth/register', data);
export const login = (data) => api.post('/Auth/login', data);

// Optimizer
export const optimize = (data) => api.post('/Optimizer/optimize', data);

// Electricity Prices
export const getPrices = () => api.get('/ElectricityPrices');

// Charging Sessions
export const getSessions = () => api.get('/ChargingSessions');

// Vehicles
export const getVehicles = () => api.get('/UserVehicles');
export const createVehicle = (data) => api.post('/UserVehicles', data);

// Stations
export const getStations = () => api.get('/ChargingStations');
export const createStation = (data) => api.post('/ChargingStations', data);
