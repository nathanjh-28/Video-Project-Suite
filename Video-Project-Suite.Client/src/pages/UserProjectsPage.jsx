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
import { Add, Edit, Delete } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { userProjectApi } from '../services';

const UserProjectsPage = () => {
    const [userProjects, setUserProjects] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadUserProjects();
    }, []);

    const loadUserProjects = async () => {
        try {
            const data = await userProjectApi.getAllUserProjectAssignments();
            console.log('Fetched user projects:', data);
            setUserProjects(data);
        } catch (error) {
            console.error('Failed to load user projects:', error);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this project assignment?')) {
            try {
                const res = await userProjectApi.deleteUserProjectAssignment(id);
                console.log('Deleted project assignment with id:', id);
                console.log(res);
                loadUserProjects();
            } catch (error) {
                console.error('Failed to delete project assignment:', error);
            }
        }
    };

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Typography color="text.primary">Project Assignments</Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                Project Assignments
            </Typography>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>UserName</TableCell>
                            <TableCell>Project Short Name</TableCell>
                            <TableCell>Role</TableCell>
                            <TableCell>Start Date</TableCell>
                            <TableCell>End Date</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {userProjects.map((userProject) => (
                            <TableRow
                                key={userProject.id}
                                hover


                            >
                                <TableCell>
                                    <Typography variant="subtitle2">{userProject.userName}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">{userProject.projectShortName}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">{userProject.role}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">{new Date(userProject.assignedAt).toLocaleDateString()}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">{userProject.removedAt ? new Date(userProject.removedAt).toLocaleDateString() : 'N/A'}</Typography>
                                </TableCell>
                                <TableCell>
                                    <IconButton
                                        size="small"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            navigate(`/user-project/${userProject.id}/edit`);
                                        }}
                                    >
                                        <Edit />
                                    </IconButton>
                                    <IconButton
                                        size="small"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleDelete(userProject.id);
                                        }}
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
                onClick={() => navigate('/userprojects/new')}
            >
                <Add />
            </Fab>
        </Box>
    );
};

export default UserProjectsPage;