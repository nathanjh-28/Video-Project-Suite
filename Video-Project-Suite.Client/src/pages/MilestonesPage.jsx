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
import { milestoneApi } from '../services';


const MilestonesPage = () => {
    const [milestones, setMilestones] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadMilestones();
    }, []);

    const loadMilestones = async () => {
        try {
            const data = await milestoneApi.getAll();
            // sort milestones by position
            data.sort((a, b) => a.position - b.position);
            setMilestones(data);
            console.log("Loaded milestones:", data);
        } catch (error) {
            console.error('Failed to load milestones:', error);
        }

    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this milestone?')) {
            try {
                await milestoneApi.delete(id);
                setMilestones(milestones.filter(m => m.id !== id));
            } catch (error) {
                console.error('Failed to delete milestone:', error);
            }
        }
    };


    return (
        <Box>
            <Breadcrumbs sx={{ mb: 2 }}>
                <Link color="inherit" href="/">Home</Link>
                <Typography color="text.primary">Milestones</Typography>
            </Breadcrumbs>

            <Typography variant="h4" gutterBottom>
                Milestones
            </Typography>

            <Box sx={{ overflowY: 'scroll', maxHeight: '70vh' }}>

                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Milestone Name</TableCell>
                                <TableCell>Position</TableCell>
                                <TableCell>Actions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {milestones.map((milestone) => (
                                <TableRow key={milestone.id} hover>
                                    <TableCell>
                                        <Typography variant="subtitle2">
                                            {milestone.name}
                                        </Typography>
                                    </TableCell>
                                    <TableCell>
                                        <Typography variant="subtitle2">
                                            {milestone.position}
                                        </Typography>
                                    </TableCell>

                                    <TableCell>
                                        <IconButton size="small" onClick={(e) => {
                                            e.stopPropagation();
                                            navigate(`/milestones/${milestone.id}/edit`);
                                        }} >
                                            <Edit />
                                        </IconButton>
                                        <IconButton
                                            size="small"
                                            onClick={() => handleDelete(milestone.id)}
                                        >
                                            <Delete />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>

            <Fab
                color="primary"
                sx={{ position: 'fixed', bottom: 16, right: 16 }}
                // navigate to register page
                onClick={() => navigate('/milestones/new')}
            >
                <Add />
            </Fab>
        </Box>
    );
}
export default MilestonesPage;