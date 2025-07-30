// src/pages/ProjectDetail.js
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
import { projectApi } from '../services/api';

const ProjectDetail = () => {
    const [project, setProject] = useState(null);
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        loadProject();
    }, [id]);

    const loadProject = async () => {
        try {
            const data = await projectApi.getById(id);
            setProject(data);
        } catch (error) {
            console.error('Failed to load project:', error);
        }
    };

    const handleDelete = async () => {
        if (window.confirm('Are you sure you want to delete this project?')) {
            try {
                await projectApi.delete(id);
                navigate('/projects');
            } catch (error) {
                console.error('Failed to delete project:', error);
            }
        }
    };

    if (!project) return <div>Loading...</div>;

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Link color="inherit" href="/projects">Projects</Link>
                <Typography color="text.primary">{project.title}</Typography>
            </Breadcrumbs>

            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4">{project.title}</Typography>
                <Box>
                    <Button
                        variant="outlined"
                        startIcon={<Edit />}
                        onClick={() => navigate(`/projects/${id}/edit`)}
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
                            <Typography variant="subtitle2" color="text.secondary">Short Name</Typography>
                            <Typography variant="body1" gutterBottom>{project.shortName}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Title</Typography>
                            <Typography variant="body1" gutterBottom>{project.title}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Type</Typography>
                            <Typography variant="body1" gutterBottom>{project.type}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Focus</Typography>
                            <Typography variant="body1" gutterBottom>{project.focus}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Status</Typography>
                            <Chip label={project.status} color="primary" size="small" sx={{ mb: 2 }} />

                            <Typography variant="subtitle2" color="text.secondary">Client</Typography>
                            <Typography variant="body1" gutterBottom>{project.client}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Producer</Typography>
                            <Typography variant="body1">{project.producer}</Typography>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Typography variant="subtitle2" color="text.secondary">Editor</Typography>
                            <Typography variant="body1" gutterBottom>{project.editor}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Quantity Per Unit</Typography>
                            <Typography variant="body1" gutterBottom>{project.qtyPerUnit}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Price Per Unit</Typography>
                            <Typography variant="body1" gutterBottom>${project.pricePerUnit}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Expense Budget</Typography>
                            <Typography variant="body1" gutterBottom>${project.expenseBudget}</Typography>

                            <Typography variant="subtitle2" color="text.secondary">Start Date</Typography>
                            <Typography variant="body1" gutterBottom>
                                {project.startDate ? new Date(project.startDate).toLocaleDateString() : 'Not set'}
                            </Typography>

                            <Typography variant="subtitle2" color="text.secondary">End Date</Typography>
                            <Typography variant="body1" gutterBottom>
                                {project.endDate ? new Date(project.endDate).toLocaleDateString() : 'Not set'}
                            </Typography>
                        </Grid>
                        <Grid item xs={12}>
                            <Typography variant="subtitle2" color="text.secondary">Scope</Typography>
                            <Typography variant="body1" gutterBottom>
                                {project.scope || 'No scope defined.'}
                            </Typography>

                            <Typography variant="subtitle2" color="text.secondary">Expense Summary</Typography>
                            <Typography variant="body1" gutterBottom>
                                {project.expenseSummary || 'No expense summary available.'}
                            </Typography>

                            <Typography variant="subtitle2" color="text.secondary">Comments</Typography>
                            <Typography variant="body1">
                                {project.comments || 'No comments available.'}
                            </Typography>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>
        </Box>
    );
};

export default ProjectDetail;