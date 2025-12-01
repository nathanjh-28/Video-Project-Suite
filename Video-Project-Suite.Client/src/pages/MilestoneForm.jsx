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
import { milestoneApi } from '../services';

const MilestoneForm = () => {
    const [milestone, setMilestone] = useState({
        name: '',
        position: ''
    });
    const [loading, setLoading] = useState(false);
    const { id } = useParams();
    const navigate = useNavigate();
    const isEditing = Boolean(id);

    useEffect(() => {
        if (isEditing) {
            loadMilestone();
        }
    }, [id, isEditing]);

    const loadMilestone = async () => {
        if (!isEditing) return;
        try {
            const data = await milestoneApi.getById(id);
            setMilestone(data);
            console.log("milestone data:\n", data);
        } catch (error) {
            console.error('Failed to load milestone:', error);
        }
    };

    const handleChange = (field) => (event) => {
        setMilestone(prev => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setLoading(true);

        try {
            if (isEditing) {
                await milestoneApi.update(id, milestone);
            } else {
                await milestoneApi.create(milestone);
            }
            navigate('/milestones');
        } catch (error) {
            console.error('Failed to save milestone:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadMilestone();
    }, []);

    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Link color="inherit" href="/milestones">Milestones</Link>
                <Typography color="text.primary">
                    {isEditing ? 'Edit Milestone' : 'New Milestone'}
                </Typography>
            </Breadcrumbs>

            <form onSubmit={handleSubmit}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h4" gutterBottom>
                        {milestone.name ? milestone.name : (isEditing ? 'Edit Milestone' : 'New Milestone')}
                    </Typography>
                    <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end', mb: 2 }}>
                        <Button
                            variant="outlined"
                            onClick={() => navigate('/milestones')}
                        >
                            Cancel
                        </Button>
                        <Button
                            type="submit"
                            variant="contained"
                            disabled={loading}
                        >
                            {loading ? 'Saving...' : 'Save Milestone'}
                        </Button>
                    </Box>
                </Box>

                <Card sx={{ overflowY: 'scroll', maxHeight: '70vh' }}>
                    <CardContent>
                        <Grid container spacing={2}>
                            <Grid item xs={12} size={12}>
                            </Grid>
                            <Grid item xs={12} md={6} size={10}>
                                <TextField
                                    fullWidth
                                    label="Name"
                                    value={milestone.name}
                                    onChange={handleChange('name')}
                                    required
                                />
                            </Grid>

                            <Grid item xs={12} md={6} size={2}>
                                <TextField
                                    fullWidth
                                    label="Position"
                                    value={milestone.position}
                                    onChange={handleChange('position')}
                                    required
                                />
                            </Grid>


                        </Grid>
                    </CardContent>
                </Card>
            </form>
        </Box>
    );
};

export default MilestoneForm;