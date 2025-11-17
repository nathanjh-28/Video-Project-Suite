// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import ProjectsPage from './pages/ProjectsPage';
import ProjectDetail from './pages/ProjectDetail';
import ProjectForm from './pages/ProjectForm';
import UserProjectForm from './pages/UserProjectForm';
import UsersPage from './pages/UsersPage';
import Login from './pages/Login';
import Register from './pages/Register';
import UsersDetail from './pages/UsersDetail';
import UserProjectsPage from './pages/UserProjectsPage';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <AuthProvider>
          <Layout>
            <Routes>
              {/* Public routes */}
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />

              {/* Protected routes */}
              <Route path="/" element={<ProtectedRoute><ProjectsPage /></ProtectedRoute>} />
              <Route path="/user-project" element={<ProtectedRoute><UserProjectsPage /></ProtectedRoute>} />
              <Route path="/user-project/:id/edit" element={<ProtectedRoute><UserProjectForm /></ProtectedRoute>} />
              <Route path="/userprojects/new" element={<ProtectedRoute><UserProjectForm /></ProtectedRoute>} />
              <Route path="/projects" element={<ProtectedRoute><ProjectsPage /></ProtectedRoute>} />
              <Route path="/projects/:id" element={<ProtectedRoute><ProjectDetail /></ProtectedRoute>} />
              <Route path="/projects/new" element={<ProtectedRoute><ProjectForm /></ProtectedRoute>} />
              <Route path="/projects/:id/edit" element={<ProtectedRoute><ProjectForm /></ProtectedRoute>} />
              <Route path="/users" element={<ProtectedRoute><UsersPage /></ProtectedRoute>} />
              <Route path="/users/:id" element={<ProtectedRoute><UsersDetail /></ProtectedRoute>} />
            </Routes>
          </Layout>
        </AuthProvider>
      </Router>
    </ThemeProvider>
  );
}

export default App;
