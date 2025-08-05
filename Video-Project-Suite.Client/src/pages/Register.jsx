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
// import { projectApi } from '../services/api';

const Register = () => {
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
                Register
            </Typography>
            {/* display flex column */}
            {/* center card */}
            <Card >
                <CardContent >
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
                                    select
                                    label="Role"
                                    value={user.role}
                                    onChange={handleChange('role')}
                                    required
                                >
                                    {roleOptions.map((option) => (
                                        <MenuItem key={option} value={option}>
                                            {option}
                                        </MenuItem>
                                    ))}
                                </TextField>
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
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Confirm Password"
                                    value={user.confirmPassword}
                                    onChange={handleChange('confirmPassword')}
                                    type="password"
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Email"
                                    value={user.email}
                                    onChange={handleChange('email')}
                                    type="email"
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="First Name"
                                    value={user.firstName}
                                    onChange={handleChange('firstName')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Last Name"
                                    value={user.lastName}
                                    onChange={handleChange('lastName')}
                                    required
                                />
                            </Grid>
                        </Grid>
                    </form>
                </CardContent>
            </Card>
        </Box >
    );
};


export default Register;