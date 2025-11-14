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
    Link,
    Button
} from '@mui/material';
import { Add, Edit, Delete } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { projectApi } from '../services';

import ProjectList from '../components/ProjectList';
import ProjectBoard from '../components/ProjectBoard';

const ProjectsPage = () => {
    const [trelloButton, setTrelloButton] = useState(false);
    const [projects, setProjects] = useState([]);
    const navigate = useNavigate();

    const handleTrelloClick = () => {
        setTrelloButton(!trelloButton);
    };

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


            {trelloButton ? (
                <ProjectBoard projects={projects} handleDelete={handleDelete} getStatusColor={getStatusColor} />
            ) : (
                <ProjectList projects={projects} handleDelete={handleDelete} getStatusColor={getStatusColor} />
            )}


            <Fab
                color="primary"
                sx={{ position: 'fixed', bottom: 16, right: 16 }}
                onClick={() => navigate('/projects/new')}
            >
                <Add />
            </Fab>

            <Fab
                color="secondary"
                sx={{ position: 'fixed', bottom: 16, right: 80 }}
                onClick={handleTrelloClick}
            >
                {trelloButton ? 'Table' : 'Kanban'}
            </Fab>

        </Box>
    );
};

export default ProjectsPage;