import React, { useState, useEffect } from 'react';
import {
    Box,
    Card,
    CardContent,
    TextField,
    Button,
    Grid,
    MenuItem,
    Typography,
    Breadcrumbs,
    Link
} from '@mui/material';


import { useParams, useNavigate } from 'react-router-dom';
import { userApi } from '../services/api';
// import { projectApi } from '../services/api';


const Login = () => {
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
    const { id } = useParams();
    const navigate = useNavigate();
    const isEditing = Boolean(id);

    useEffect(() => {
        if (isEditing) {
            loadProject();
        }
    }, [id, isEditing]);

    const handleChange = (field) => (event) => {
        setUser(prev => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        userApi.login(user)
            .then(() => {
                alert('Login successful!');
                navigate('/');
            })
            .catch((error) => {
                console.error('Login failed:', error);
                alert('Login failed. Please try again.');
            });
    };

    const roleOptions = [
        'Producer',
        'Client',
        'Editor',
        'N/A'
    ];


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
