// src/pages/UsersPage.js
import React, { useState, useEffect } from 'react';
import {
    Box,
    Typography,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    Chip,
    IconButton,
    Fab,
    Breadcrumbs,
    Link
} from '@mui/material';
import { PersonAdd, Edit, Delete } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { userApi } from '../services/api';

const UsersPage = () => {
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadUsers();
    }, []);

    const loadUsers = async () => {
        try {
            // Replace with actual API call
            // const mockUsers = [
            //     { id: 1, userName: 'jd1', firstName: 'John', lastName: 'Doe', email: 'john@example.com', role: 'Admin', status: 'Active' },
            //     { id: 2, userName: 'jane9', firstName: 'Jane', lastName: 'Smith', email: 'jane@example.com', role: 'Producer', status: 'Active' },
            //     { id: 3, userName: 'bobj5', firstName: 'Bob', lastName: 'Johnson', email: 'bob@example.com', role: 'Client', status: 'Active' },
            //     { id: 4, userName: 'steve7', firstName: 'Steve', lastName: 'Martin', email: 'steve@example.com', role: 'Editor', status: 'Active' }
            // ];
            const data = await userApi.getAll();
            setUsers(data);
            console.log("Loaded users:", data);
        } catch (error) {
            console.error('Failed to load users:', error);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            try {
                setUsers(users.filter(u => u.id !== id));
            } catch (error) {
                console.error('Failed to delete user:', error);
            }
        }
    };

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Typography color="text.primary">Users</Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                Users
            </Typography>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Username</TableCell>
                            <TableCell>First Name</TableCell>
                            <TableCell>Last Name</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Role</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {users.map((user) => (
                            <TableRow key={user.id} hover onClick={() => navigate(`/users/${user.id}`)}>
                                <TableCell>
                                    <Typography variant="subtitle2">
                                        {user.username}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">
                                        {user.firstName}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">
                                        {user.lastName}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">
                                        {user.email}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Chip
                                        label={user.role}
                                        color={user.role === 'admin' ? 'primary' : 'default'}
                                        size="small"
                                    />
                                </TableCell>
                                <TableCell>
                                    <Chip
                                        label={user.status}
                                        color="success"
                                        size="small"
                                    />
                                </TableCell>
                                <TableCell>
                                    <IconButton size="small">
                                        <Edit />
                                    </IconButton>
                                    <IconButton
                                        size="small"
                                        onClick={() => handleDelete(user.id)}
                                    >
                                        <Delete />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Fab
                color="primary"
                sx={{ position: 'fixed', bottom: 16, right: 16 }}
                // navigate to register page
                onClick={() => navigate('/register')}
            >
                <PersonAdd />
            </Fab>
        </Box>
    );
};

export default UsersPage;