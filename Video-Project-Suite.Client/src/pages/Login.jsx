import React, { useState } from 'react';
import {
    Box,
    Card,
    CardContent,
    TextField,
    Button,
    Grid,
    Typography
} from '@mui/material';


import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Login = () => {
    const { login } = useAuth();
    const [user, setUser] = useState({
        username: '',
        password: '',
        confirmPassword: '',
        firstName: '',
        lastName: '',
        email: '',
        role: '',
    });
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleChange = (field) => (event) => {
        setUser(prev => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setLoading(true);
        try {
            await login(user);
            alert('Login successful!');
            navigate('/');
        } catch (error) {
            console.error('Login failed:', error);
            alert('Login failed. Please try again.');
        } finally {
            setLoading(false);
        }
    };

    return (

        <Box maxWidth={600} sx={{ margin: 'auto', mt: 2 }}>
            <Typography variant="h2" gutterBottom>
                Login
            </Typography>
            {/* display flex column */}
            <Card>
                <CardContent>
                    <form onSubmit={handleSubmit}>
                        {/* Grid must be vertical column of elements */}
                        <Grid container spacing={3} direction="column">
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="User Name"
                                    value={user.username}
                                    onChange={handleChange('username')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Password"
                                    value={user.password}
                                    onChange={handleChange('password')}
                                    type="password"
                                    required
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <Button
                                    type="submit"
                                    variant="contained"
                                    color="primary"
                                    fullWidth
                                    disabled={loading}
                                >
                                    {loading ? 'Logging in...' : 'Login'}
                                </Button>
                            </Grid>
                        </Grid>
                    </form>
                </CardContent>
            </Card>
        </Box >
    );
};

export default Login;
