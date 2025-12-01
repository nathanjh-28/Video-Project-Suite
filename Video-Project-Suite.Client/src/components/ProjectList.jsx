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
import { milestoneApi } from '../services';

import { useNavigate } from 'react-router-dom';


const ProjectList = ({ projects, handleDelete, getMilestoneColor }) => {

    const navigate = useNavigate();
    const [milestones, setMilestones] = useState([]);

    useEffect(() => {
        loadMilestones();
    }, []);

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

    return (
        <Box sx={{ overflowY: 'scroll', maxHeight: '70vh' }}>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Project Title</TableCell>
                            <TableCell>Type</TableCell>
                            <TableCell>Milestone</TableCell>
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
                                        label={milestones[project.milestoneId - 1].name}
                                        color={getMilestoneColor(project.milestoneId)}
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
            </TableContainer >
        </Box >
    )
}

export default ProjectList;