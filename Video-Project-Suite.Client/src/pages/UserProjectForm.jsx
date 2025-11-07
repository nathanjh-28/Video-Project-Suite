// src/pages/UserProjectForm.js
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
import { userProjectApi } from '../services';

const UserProjectForm = () => {
    const [userProject, setUserProject] = useState({
        userId: '',
        projectId: '',
        assignedAt: '',
        removedAt: '',
        userName: '',
        projectShortName: '',
        role: '',
    });

    const [loading, setLoading] = useState(false);
    const { id } = useParams();
    const navigate = useNavigate();
    const isEditing = Boolean(id);

    useEffect(() => {
        if (isEditing) {
            loadUserProject();
        }
    }, [id, isEditing]);

    const loadUserProject = async () => {
        try {
            const data = await userProjectApi.getUserProjectAssignmentById(id);
            setUserProject(data);
            console.log("user project data:\n", data);
        } catch (error) {
            console.error('Failed to load user project:', error);
        }
    };

    const handleChange = (field) => (event) => {
        setUserProject(prev => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setLoading(true);

        try {
            if (isEditing) {
                console.log('Updating user project with data:', userProject);
                await userProjectApi.updateUserProjectAssignment(id, userProject);
            } else {
                console.log('Creating user project with data:', userProject);
                await userProjectApi.createUserProjectAssignment(userProject);
            }
            navigate('/user-project');
        } catch (error) {
            console.error('Failed to save project:', error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Link color="inherit" href="/user-project">Project Assignments</Link>
                <Typography color="text.primary">
                    {isEditing ? 'Edit Project Assignment' : 'New Project Assignment'}
                </Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                {isEditing ? 'Edit Project Assignment' : 'New Project Assignment'}
            </Typography>

            <Card>
                <CardContent>
                    <form onSubmit={handleSubmit}>
                        <Grid container spacing={3}>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="User ID"
                                    value={userProject.userId}
                                    onChange={handleChange('userId')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Project ID"
                                    value={userProject.projectId}
                                    onChange={handleChange('projectId')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Role"
                                    value={userProject.role}
                                    onChange={handleChange('role')}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    value={userProject.assignedAt}
                                    label="Assigned At"
                                    onChange={handleChange('assignedAt')}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    required
                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    label="Removed At"
                                    value={userProject.RemovedAt}
                                    onChange={handleChange('removedAt')}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}

                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="User Name"
                                    value={userProject.userName}
                                    onChange={handleChange('userName')}

                                />
                            </Grid>
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    label="Project Short Name"
                                    value={userProject.projectShortName}
                                    onChange={handleChange('projectShortName')}

                                />
                            </Grid>

                            <Grid item xs={12}>
                                <Box sx={{ display: 'flex', gap: 2 }}>
                                    <Button
                                        variant="outlined"
                                        onClick={() => navigate('/user-project')}
                                    >
                                        Cancel
                                    </Button>
                                    <Button
                                        type="submit"
                                        variant="contained"
                                        disabled={loading}
                                    >
                                        {loading ? 'Saving...' : 'Submit'}
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

export default UserProjectForm;