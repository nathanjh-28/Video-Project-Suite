// src/pages/UserDetail.js
import React, { useState, useEffect } from 'react';
import {
    Box,
    Typography,
    Card,
    CardContent,
    Grid,
    Chip,
    Button,
    Breadcrumbs,
    Link
} from '@mui/material';
import { Edit, Delete } from '@mui/icons-material';
import { useParams, useNavigate } from 'react-router-dom';
import { userApi } from '../services/api';


const UsersDetail = () => {
    const [user, setUser] = useState(null);
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        loadUser();
    }, [id]);

    const loadUser = async () => {
        try {
            const data = await userApi.getById(id);
            setUser(data);
        } catch (error) {
            console.error('Failed to load user:', error);
        }
    };

    const handleDelete = async () => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            try {
                await userApi.delete(id);
                navigate('/users');
            } catch (error) {
                console.error('Failed to delete user:', error);
            }
        }
    };

    if (!user) return <div>Loading...</div>;

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Link color="inherit" href="/users">Users</Link>
                <Typography color="text.primary">{user.username}</Typography>
            </Breadcrumbs>

            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4">{user.username}</Typography>
                <Box>
                    <Button
                        variant="outlined"
                        startIcon={<Edit />}
                        onClick={() => navigate(`/users/${id}/edit`)}
                        sx={{ mr: 1 }}
                    >
                        Edit
                    </Button>
                    <Button
                        variant="contained"
                        color="error"
                        startIcon={<Delete />}
                        onClick={handleDelete}
                    >
                        Delete
                    </Button>
                </Box>
            </Box>

            <Card>
                <CardContent>
                    <Grid container spacing={3}>
                        <Grid item xs={12} md={6}>
                            <Typography variant="subtitle2" color="text.secondary">Username</Typography>
                            <Typography variant="body1" gutterBottom>{user.username}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">First Name</Typography>
                            <Typography variant="body1" gutterBottom>{user.firstName}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Last Name</Typography>
                            <Typography variant="body1" gutterBottom>{user.lastName}</Typography>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Typography variant="subtitle2" color="text.secondary">Email</Typography>
                            <Typography variant="body1" gutterBottom>{user.email}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Role</Typography>
                            <Chip label={user.role} color="primary" size="small" sx={{ mb: 2 }} />
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>
        </Box>
    );
};

export default UsersDetail;