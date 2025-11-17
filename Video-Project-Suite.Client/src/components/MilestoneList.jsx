import React, { useState, useEffect } from 'react';
import {
    AppBar,
    Box,
    Drawer,
    Fab,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
    Typography,
    useMediaQuery,
    useTheme,
    Card,
    CardHeader,
    Paper
} from '@mui/material';
import { Add, Edit, Delete } from '@mui/icons-material';

import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd';



import { useNavigate } from 'react-router-dom';

/* 
mock data
    mock milestones
    mock projects
*/

const milestonesList = ({ milestone, projects }) => {
    const navigate = useNavigate();
    // get projects with given milestone

    // use state

    // helper functions

    return (
        // Droppable area

        // map projects with draggable elements

        <Card sx={{ minWidth: 300, maxWidth: 300, minHeight: 400 }}>
            <CardHeader title={milestone.name} />

            <Droppable key={milestone.id} droppableId={milestone.id.toString()}>
                {(provided) => (
                    <Box
                        ref={provided.innerRef}
                        {...provided.droppableProps} >


                        {projects.map((project, index) => (
                            <Draggable draggableId={project.project_id.toString()} key={project.project_id} index={index}>

                                {(provided) => (
                                    <Box
                                        ref={provided.innerRef}
                                        {...provided.draggableProps}
                                        {...provided.dragHandleProps}>

                                        <Card sx={{ margin: 1, padding: 1, cursor: 'pointer', '&:hover': { padding: 1.5 } }} onClick={() => navigate(`/projects/${project.project_id}`)} key={project.project_id}>

                                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                                <Typography variant="subtitle2">{project.project_short_name}</Typography>

                                                <IconButton
                                                    size="small"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        navigate(`/projects/${project.project_id}/edit`);
                                                    }}

                                                >
                                                    <Edit />
                                                </IconButton>
                                            </Box>
                                        </Card>
                                    </Box>
                                )}
                            </Draggable>
                        ))}
                        {provided.placeholder}
                    </Box>
                )}
            </Droppable>
        </Card >
    );
};

export default milestonesList;