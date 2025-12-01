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
import { projectApi, milestoneApi } from '../services';

import ProjectList from '../components/ProjectList';
import ProjectBoard from '../components/ProjectBoard';

const ProjectsPage = () => {
    const [trelloButton, setTrelloButton] = useState(false);
    const [projects, setProjects] = useState([]);
    const [milestones, setMilestones] = useState([]);
    const navigate = useNavigate();

    const handleTrelloClick = () => {
        setTrelloButton(!trelloButton);
    };

    useEffect(() => {
        loadProjects();
        loadMilestones();
    }, []);

    const loadProjects = async () => {
        try {
            const data = await projectApi.getAll();
            setProjects(data);
        } catch (error) {
            console.error('Failed to load projects:', error);
        }
    };

    const loadMilestones = async () => {
        try {
            const data = await milestoneApi.getAll();
            setMilestones(data);
            return data;
        } catch (error) {
            console.error('Failed to load milestones:', error);
            return [];
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

    const getMilestoneColor = (milestone) => {
        switch (milestone) {
            case 1: return 'primary'; // prospect
            case 2: return 'warning'; // bidding
            case 3: return 'secondary'; // preproduction
            case 4: return 'default'; // production
            case 5: return 'secondary'; // post production
            case 6: return 'success'; // invoiced
            case 7: return 'default'; // completed
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
                <ProjectBoard projects={projects} handleDelete={handleDelete} getMilestoneColor={getMilestoneColor} />
            ) : (
                <ProjectList projects={projects} handleDelete={handleDelete} getMilestoneColor={getMilestoneColor} navigate={navigate} milestones={milestones} />
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