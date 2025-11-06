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
import { projectApi } from '../services';

const ProjectsPage = () => {
    const [projects, setProjects] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadProjects();
    }, []);

    const loadProjects = async () => {
        try {
            const data = await projectApi.getAll();
            setProjects(data);
        } catch (error) {
            console.error('Failed to load projects:', error);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this project?')) {
            try {
                await projectApi.delete(id);
                loadProjects();
            } catch (error) {
                console.error('Failed to delete project:', error);
            }
        }
    };

    const getStatusColor = (status) => {
        switch (status.toLowerCase()) {
            case 'completed': return 'success';
            case 'in progress': return 'info';
            case 'on hold': return 'warning';
            default: return 'default';
        }
    };

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Typography color="text.primary">Projects</Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                Projects
            </Typography>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Project Title</TableCell>
                            <TableCell>Type</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Budget</TableCell>
                            <TableCell>Start Date</TableCell>
                            <TableCell>End Date</TableCell>
                            {/* <TableCell>Producer</TableCell>
                            <TableCell>Client</TableCell>
                            <TableCell>Editor</TableCell> */}
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {projects.map((project) => (
                            <TableRow
                                key={project.id}
                                hover
                                sx={{ cursor: 'pointer' }}
                                onClick={() => navigate(`/projects/${project.id}`)}
                            >
                                <TableCell>
                                    <Typography variant="subtitle2">{project.title}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Typography variant="subtitle2">{project.type}</Typography>
                                </TableCell>
                                <TableCell>
                                    <Chip
                                        label={project.status}
                                        color={getStatusColor(project.status)}
                                        size="small"
                                    />
                                </TableCell>
                                <TableCell>${project.expenseBudget}</TableCell>
                                <TableCell>{new Date(project.startDate).toLocaleDateString()}</TableCell>
                                <TableCell>{new Date(project.endDate).toLocaleDateString()}</TableCell>
                                <TableCell>
                                    <IconButton
                                        size="small"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            navigate(`/projects/${project.id}/edit`);
                                        }}
                                    >
                                        <Edit />
                                    </IconButton>
                                    <IconButton
                                        size="small"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleDelete(project.id);
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
                onClick={() => navigate('/projects/new')}
            >
                <Add />
            </Fab>
        </Box>
    );
};

export default ProjectsPage;