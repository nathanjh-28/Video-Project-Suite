// src/pages/ProjectForm.js
import React, { useState, useEffect } from 'react';
import {
    Box,
    Typography,
    Card,
    CardContent,
    TextField,
    Button,
    Grid,
    MenuItem,
    Breadcrumbs,
    Link
} from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import { projectApi } from '../services';

const ProjectForm = () => {
    const [project, setProject] = useState({
        shortName: '',
        title: '',
        type: 'web',
        focus: '',
        status: 'Planning',
        client: '',
        producer: '',
        qtyPerUnit: 1,
        pricePerUnit: 0,
        editor: '',
        expenseBudget: 0,
        expenseSummary: '',
        comments: '',
        scope: '',
        startDate: '',
        endDate: ''
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

    const loadProject = async () => {
        try {
            const data = await projectApi.getById(id);
            setProject(data);
            console.log("project data:\n", data);
        } catch (error) {
            console.error('Failed to load project:', error);
        }
    };

    const handleChange = (field) => (event) => {
        setProject(prev => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setLoading(true);

        try {
            if (isEditing) {
                await projectApi.update(id, project);
            } else {
                await projectApi.create(project);
            }
            navigate('/projects');
        } catch (error) {
            console.error('Failed to save project:', error);
        } finally {
            setLoading(false);
        }
    };

    const statusOptions = [
        'Planning',
        'In Progress',
        'On Hold',
        'Completed'
    ];

    const typeOptions = [
        'Photo',
        'Video',
        'Photo+Video',
        'Branded Marketing Campaign',
        'other'
    ];

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Link color="inherit" href="/projects">Projects</Link>
                <Typography color="text.primary">
                    {isEditing ? 'Edit Project' : 'New Project'}
                </Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                {isEditing ? 'Edit Project' : 'New Project'}
            </Typography>

            <Card>
                <CardContent>
                    <form onSubmit={handleSubmit}>
                        <Grid container spacing={3}>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Short Name"
                                    value={project.shortName}
                                    onChange={handleChange('shortName')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Title"
                                    value={project.title}
                                    onChange={handleChange('title')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    select
                                    label="Type"
                                    value={project.type}
                                    onChange={handleChange('type')}
                                >
                                    {typeOptions.map((option) => (
                                        <MenuItem key={option} value={option}>
                                            {option}
                                        </MenuItem>
                                    ))}
                                </TextField>
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Focus"
                                    value={project.focus}
                                    onChange={handleChange('focus')}
                                    placeholder="e.g., UI/UX, Backend, Frontend"
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    select
                                    label="Status"
                                    value={project.status}
                                    onChange={handleChange('status')}
                                >
                                    {statusOptions.map((option) => (
                                        <MenuItem key={option} value={option}>
                                            {option}
                                        </MenuItem>
                                    ))}
                                </TextField>
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Client"
                                    value={project.client}
                                    onChange={handleChange('client')}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Producer"
                                    value={project.producer}
                                    onChange={handleChange('producer')}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Editor"
                                    value={project.editor}
                                    onChange={handleChange('editor')}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="number"
                                    label="Quantity Per Unit"
                                    value={project.qtyPerUnit}
                                    onChange={handleChange('qtyPerUnit')}
                                    inputProps={{ min: 1 }}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="number"
                                    label="Price Per Unit"
                                    value={project.pricePerUnit}
                                    onChange={handleChange('pricePerUnit')}
                                    inputProps={{ min: 0, step: 0.01 }}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="number"
                                    label="Expense Budget"
                                    value={project.expenseBudget}
                                    onChange={handleChange('expenseBudget')}
                                    inputProps={{ min: 0 }}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    label="Start Date"
                                    value={project.startDate}
                                    onChange={handleChange('startDate')}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    label="End Date"
                                    value={project.endDate}
                                    onChange={handleChange('endDate')}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={3}
                                    label="Scope"
                                    value={project.scope}
                                    onChange={handleChange('scope')}
                                    placeholder="Enter project scope..."
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={2}
                                    label="Expense Summary"
                                    value={project.expenseSummary}
                                    onChange={handleChange('expenseSummary')}
                                    placeholder="e.g., travel, food 5 days, software licenses"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={2}
                                    label="Comments"
                                    value={project.comments}
                                    onChange={handleChange('comments')}
                                    placeholder="Enter any additional comments..."
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <Box sx={{ display: 'flex', gap: 2 }}>
                                    <Button
                                        variant="outlined"
                                        onClick={() => navigate('/projects')}
                                    >
                                        Cancel
                                    </Button>
                                    <Button
                                        type="submit"
                                        variant="contained"
                                        disabled={loading}
                                    >
                                        {loading ? 'Saving...' : 'Save Project'}
                                    </Button>
                                </Box>
                            </Grid>
                        </Grid>
                    </form>
                </CardContent>
            </Card>
        </Box>
    );
};

export default ProjectForm;